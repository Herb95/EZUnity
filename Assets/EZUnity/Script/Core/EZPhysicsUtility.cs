/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-18 20:43:20
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZUnity
{
    public static partial class EZPhysicsUtility
    {
        public static void GetCapsuleSpheres(CapsuleCollider collider, out Vector3 sphereP0, out Vector3 sphereP1, out float radius)
        {
            Vector3 scale = collider.transform.localScale.Abs();
            radius = collider.radius;
            sphereP0 = sphereP1 = collider.center;
            float height = collider.height * 0.5f;
            switch (collider.direction)
            {
                case 0:
                    radius *= Mathf.Max(scale.y, scale.z);
                    height = Mathf.Max(0, height - radius / scale.x);
                    sphereP0.x -= height;
                    sphereP1.x += height;
                    break;
                case 1:
                    radius *= Mathf.Max(scale.x, scale.z);
                    height = Mathf.Max(0, height - radius / scale.y);
                    sphereP0.y -= height;
                    sphereP1.y += height;
                    break;
                case 2:
                    radius *= Mathf.Max(scale.x, scale.y);
                    height = Mathf.Max(0, height - radius / scale.z);
                    sphereP0.z -= height;
                    sphereP1.z += height;
                    break;
            }
            sphereP0 = collider.transform.TransformPoint(sphereP0);
            sphereP1 = collider.transform.TransformPoint(sphereP1);
        }

        public static void PointOutsideSphere(ref Vector3 position, SphereCollider collider, float spacing)
        {
            Vector3 scale = collider.transform.localScale.Abs();
            float radius = collider.radius * Mathf.Max(scale.x, scale.y, scale.z);
            PointOutsideSphere(ref position, collider.transform.TransformPoint(collider.center), radius + spacing);
        }
        public static void PointOutsideSphere(ref Vector3 position, Vector3 spherePosition, float radius)
        {
            Vector3 bounceDir = position - spherePosition;
            if (bounceDir.magnitude < radius)
            {
                position = spherePosition + bounceDir.normalized * radius;
            }
        }

        public static void PointInsideSphere(ref Vector3 position, SphereCollider collider, float spacing)
        {
            PointInsideSphere(ref position, collider.transform.TransformPoint(collider.center), collider.radius - spacing);
        }
        public static void PointInsideSphere(ref Vector3 position, Vector3 spherePosition, float radius)
        {
            Vector3 bounceDir = position - spherePosition;
            if (bounceDir.magnitude > radius)
            {
                position = spherePosition + bounceDir.normalized * radius;
            }
        }

        public static void PointOutsideCapsule(ref Vector3 position, CapsuleCollider collider, float spacing)
        {
            Vector3 sphereP0, sphereP1;
            float radius;
            GetCapsuleSpheres(collider, out sphereP0, out sphereP1, out radius);
            PointOutsideCapsule(ref position, sphereP0, sphereP1, radius + spacing);
        }
        public static void PointOutsideCapsule(ref Vector3 position, Vector3 sphereP0, Vector3 sphereP1, float radius)
        {
            Vector3 capsuleDir = sphereP1 - sphereP0;
            Vector3 pointDir = position - sphereP0;

            float dot = Vector3.Dot(capsuleDir, pointDir);
            if (dot <= 0)
            {
                PointOutsideSphere(ref position, sphereP0, radius);
            }
            else if (dot >= capsuleDir.sqrMagnitude)
            {
                PointOutsideSphere(ref position, sphereP1, radius);
            }
            else
            {
                Vector3 bounceDir = pointDir - capsuleDir.normalized * dot / capsuleDir.magnitude;
                float bounceDis = bounceDir.magnitude;
                if (bounceDis < radius)
                {
                    position += bounceDir.normalized * (radius - bounceDis);
                }
            }
        }

        public static void PointInsideCapsule(ref Vector3 position, CapsuleCollider collider, float spacing)
        {
            Vector3 sphereP0, sphereP1;
            float radius;
            GetCapsuleSpheres(collider, out sphereP0, out sphereP1, out radius);
            PointInsideCapsule(ref position, sphereP0, sphereP1, radius - spacing);
        }
        public static void PointInsideCapsule(ref Vector3 position, Vector3 sphereP0, Vector3 sphereP1, float radius)
        {
            Vector3 capsuleDir = sphereP1 - sphereP0;
            Vector3 pointDir = position - sphereP0;

            float dot = Vector3.Dot(capsuleDir, pointDir);
            if (dot <= 0)
            {
                PointInsideSphere(ref position, sphereP0, radius);
            }
            else if (dot >= capsuleDir.sqrMagnitude)
            {
                PointInsideSphere(ref position, sphereP1, radius);
            }
            else
            {
                Vector3 bounceDir = pointDir - capsuleDir.normalized * dot / capsuleDir.magnitude;
                float bounceDis = bounceDir.magnitude;
                if (bounceDis > radius)
                {
                    position += bounceDir.normalized * (radius - bounceDis);
                }
            }
        }

        public static void PointOutsideBox(ref Vector3 position, BoxCollider collider, float spacing)
        {
            Vector3 positionToCollider = collider.transform.InverseTransformPoint(position) - collider.center;
            PointOutsideBox(ref positionToCollider, collider.size.Abs() / 2 + collider.transform.InverseTransformVector(Vector3.one * spacing).Abs());
            position = collider.transform.TransformPoint(collider.center + positionToCollider);
        }
        public static void PointOutsideBox(ref Vector3 position, Vector3 boxSize)
        {
            Vector3 distanceToCenter = position.Abs();
            if (distanceToCenter.x < boxSize.x && distanceToCenter.y < boxSize.y && distanceToCenter.z < boxSize.z)
            {
                Vector3 distance = (distanceToCenter - boxSize).Abs();
                if (distance.x < distance.y)
                {
                    if (distance.x < distance.z)
                    {
                        position.x = Mathf.Sign(position.x) * boxSize.x;
                    }
                    else
                    {
                        position.z = Mathf.Sign(position.z) * boxSize.z;
                    }
                }
                else
                {
                    if (distance.y < distance.z)
                    {
                        position.y = Mathf.Sign(position.y) * boxSize.y;
                    }
                    else
                    {
                        position.z = Mathf.Sign(position.z) * boxSize.z;
                    }
                }
            }
        }

        public static void PointInsideBox(ref Vector3 position, BoxCollider collider, float spacing)
        {
            Vector3 positionToCollider = collider.transform.InverseTransformPoint(position) - collider.center;
            PointInsideBox(ref positionToCollider, collider.size.Abs() / 2 - collider.transform.InverseTransformVector(Vector3.one * spacing).Abs());
            position = collider.transform.TransformPoint(collider.center + positionToCollider);
        }
        public static void PointInsideBox(ref Vector3 position, Vector3 boxSize)
        {
            Vector3 distanceToCenter = position.Abs();
            if (distanceToCenter.x > boxSize.x) position.x = Mathf.Sign(position.x) * boxSize.x;
            if (distanceToCenter.y > boxSize.y) position.y = Mathf.Sign(position.y) * boxSize.y;
            if (distanceToCenter.z > boxSize.z) position.z = Mathf.Sign(position.z) * boxSize.z;
        }

        public static void PointOutsideCollider(ref Vector3 position, Collider collider, float spacing)
        {
            Vector3 closestPoint = collider.ClosestPoint(position);
            if (position == closestPoint) // inside collider
            {
                Vector3 bounceDir = position - collider.bounds.center;
                Debug.DrawLine(collider.bounds.center, closestPoint, Color.red);
                position = closestPoint + bounceDir.normalized * spacing;
            }
            else
            {
                Vector3 bounceDir = position - closestPoint;
                if (bounceDir.magnitude < spacing)
                {
                    position = closestPoint + bounceDir.normalized * spacing;
                }
            }
        }
    }
}
