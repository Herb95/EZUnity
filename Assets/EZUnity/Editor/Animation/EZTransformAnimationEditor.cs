/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-11-02 17:30:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEditor;
using UnityEngine;

namespace EZUnity.Animation
{
    [CustomEditor(typeof(EZTransformAnimation), true), CanEditMultipleObjects]
    public class EZTransformAnimationEditor : EZAnimationEditor
    {
        private SerializedProperty m_PositionDriver;
        private SerializedProperty m_RotationDriver;
        private SerializedProperty m_ScaleDriver;

        private bool drivePosition;
        private bool driveRotation;
        private bool driveScale;

        private bool originFoldout = true;

        private Color positionColor = Color.red;
        private Color rotationColor = Color.green;
        private Color scaleColor = Color.blue;

        protected override void OnEnable()
        {
            m_PositionDriver = serializedObject.FindProperty("m_PositionDriver");
            m_RotationDriver = serializedObject.FindProperty("m_RotationDriver");
            m_ScaleDriver = serializedObject.FindProperty("m_ScaleDriver");
            base.OnEnable();
            segments.elementHeightCallback = GetPhaseListElementHeight;
        }

        protected override void DrawSegmentListHeader(Rect rect)
        {
            rect.x += headerIndent; rect.width -= headerIndent; rect.y += 1;
            float width = rect.width / 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "Start Value");
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, width - space, lineHeight), "End Value");
        }

        private void GetDriver()
        {
            drivePosition = m_PositionDriver.FindPropertyRelative("x").boolValue
                || m_PositionDriver.FindPropertyRelative("y").boolValue
                || m_PositionDriver.FindPropertyRelative("z").boolValue;
            driveRotation = m_RotationDriver.FindPropertyRelative("x").boolValue
                || m_RotationDriver.FindPropertyRelative("y").boolValue
                || m_RotationDriver.FindPropertyRelative("z").boolValue;
            driveScale = m_ScaleDriver.FindPropertyRelative("x").boolValue
                || m_ScaleDriver.FindPropertyRelative("y").boolValue
                || m_ScaleDriver.FindPropertyRelative("z").boolValue;
        }
        private int GetLineCount()
        {
            int line = 1;
            if (drivePosition) line++;
            if (driveRotation) line++;
            if (driveScale) line++;
            return line;
        }
        private float GetPhaseListElementHeight(int index)
        {
            GetDriver();
            return GetLineCount() * segments.elementHeight;
        }
        protected override void DrawSegmentListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Color curveColor = anim.segmentIndex == index ? Color.red : Color.green;

            SerializedProperty phase = segments.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty startValue = phase.FindPropertyRelative("m_StartValue");
            SerializedProperty endValue = phase.FindPropertyRelative("m_EndValue");
            SerializedProperty duration = phase.FindPropertyRelative("m_Duration");
            SerializedProperty curve = phase.FindPropertyRelative("m_Curve");

            rect.y += 1; float width = rect.width / 2;

            float x1 = rect.x, x2 = rect.x + width, space = 10;
            Color bgColor = GUI.backgroundColor;
            if (drivePosition)
            {
                GUI.backgroundColor = positionColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_Position"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_Position"), GUIContent.none);
                rect.y += segments.elementHeight;
            }
            if (driveRotation)
            {
                GUI.backgroundColor = rotationColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_Rotation"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_Rotation"), GUIContent.none);
                rect.y += segments.elementHeight;
            }
            if (driveScale)
            {
                GUI.backgroundColor = scaleColor;
                EditorGUI.PropertyField(new Rect(x1, rect.y, width - space, lineHeight), startValue.FindPropertyRelative("m_Scale"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(x2, rect.y, width - space, lineHeight), endValue.FindPropertyRelative("m_Scale"), GUIContent.none);
                rect.y += segments.elementHeight;
            }
            GUI.backgroundColor = bgColor;

            float labelWidth = 60; float propertyWidth = width - labelWidth - space;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, lineHeight), "Duration");
            EditorGUI.PropertyField(new Rect(rect.x + labelWidth, rect.y, propertyWidth, lineHeight), duration, GUIContent.none);
            rect.x += width;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, lineHeight), "Curve");
            curve.animationCurveValue = EditorGUI.CurveField(new Rect(rect.x + labelWidth, rect.y, propertyWidth, lineHeight), curve.animationCurveValue, curveColor, new Rect(0, 0, duration.floatValue, 1));
        }

        public override void DrawSegmentList()
        {
            originFoldout = EditorGUILayout.Foldout(originFoldout, "Driver");
            if (originFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_PositionDriver);
                EditorGUILayout.PropertyField(m_RotationDriver);
                EditorGUILayout.PropertyField(m_ScaleDriver);
                EditorGUI.indentLevel--;
            }
            segments.DoLayoutList();
        }
    }
}