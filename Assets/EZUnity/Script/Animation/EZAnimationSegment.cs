/* Author:          ezhex1991@outlook.com
 * CreateTime:      2017-12-11 17:39:15
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity.Animation
{
    public abstract class EZAnimationSegment<T>
        where T : struct
    {
        [SerializeField]
        private T m_StartValue;
        public T startValue { get { return m_StartValue; } set { m_EndValue = value; } }
        [SerializeField]
        private T m_EndValue;
        public T endValue { get { return m_EndValue; } set { m_EndValue = value; } }
        [SerializeField]
        private float m_Duration;
        public float duration { get { return m_Duration; } set { m_Duration = value >= 0 ? value : 0; } }
        [SerializeField]
        private AnimationCurve m_Curve;
        public AnimationCurve curve { get { return m_Curve; } set { m_Curve = value; } }
    }
}