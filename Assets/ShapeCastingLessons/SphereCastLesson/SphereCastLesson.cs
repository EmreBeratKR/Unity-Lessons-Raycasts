using UnityEngine;

namespace SphereCastLesson
{
    public class SphereCastLesson : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField, Min(0f)] private float sphereRadius;
        [SerializeField, Min(0f)] private float maxDistance;
        [SerializeField] private SphereCastType raycastType;


        private enum SphereCastType
        {
            SphereCast,
            SphereCastAll,
            SphereCastAllDoubleSided,
        }


        private void SphereCast(Vector3 origin, Vector3 direction)
        {
            var isHit = Physics.SphereCast(origin, sphereRadius, direction, out var hitInfo, maxDistance, layerMask);

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * hitInfo.distance);
                Gizmos.DrawSphere(hitInfo.point, 0.05f);
                Gizmos.DrawWireSphere(origin + direction * hitInfo.distance, sphereRadius);
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxDistance);
                Gizmos.DrawWireSphere(origin + direction * maxDistance, sphereRadius);
            }
        }

        private void SphereCastAll(Vector3 origin, Vector3 direction)
        {
            var hitInfos = Physics.SphereCastAll(origin, sphereRadius, direction, maxDistance, layerMask);

            var isHit = hitInfos.Length > 0;

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * maxDistance);

                foreach (var hitInfo in hitInfos)
                {
                    Gizmos.DrawSphere(hitInfo.point, 0.05f);
                    Gizmos.DrawWireSphere(origin + direction * hitInfo.distance, sphereRadius);
                }
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxDistance);
                Gizmos.DrawWireSphere(origin + direction * maxDistance, sphereRadius);
            }
        }

        private void SphereCastAllDoubleSided(Vector3 origin, Vector3 direction)
        {
            SphereCastAll(origin, direction);
            SphereCastAll(origin + direction * maxDistance, -direction);
        }


        private void OnDrawGizmos()
        {
            var origin = transform.position;
            var direction = transform.forward;

            switch (raycastType)
            {
                case SphereCastType.SphereCast:
                    SphereCast(origin, direction);
                    break;
                
                case SphereCastType.SphereCastAll:
                    SphereCastAll(origin, direction);
                    break;
                
                case SphereCastType.SphereCastAllDoubleSided:
                    SphereCastAllDoubleSided(origin, direction);
                    break;
            }
        }
    }
}
