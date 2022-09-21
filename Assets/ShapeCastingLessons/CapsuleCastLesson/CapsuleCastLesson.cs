using UnityEngine;

namespace CapsuleCastLesson
{
    public class CapsuleCastLesson : MonoBehaviour
    {
        [SerializeField] private Mesh capsuleMesh;
        [SerializeField] private LayerMask layerMask;
        [SerializeField, Min(0f)] private float capsuleHeight;
        [SerializeField, Min(0f)] private float capsuleRadius;
        [SerializeField, Min(0f)] private float maxDistance;
        [SerializeField] private CapsuleCastType raycastType;


        private enum CapsuleCastType
        {
            SphereCast,
            SphereCastAll,
            SphereCastAllDoubleSided,
        }


        private Vector3 CapsuleSize => new Vector3(capsuleRadius * 2f, capsuleHeight * 0.5f, capsuleRadius * 2f);
        

        private void SphereCast(Vector3 point1, Vector3 point2, Vector3 direction)
        {
            var origin = Vector3.Lerp(point1, point2, 0.5f);
            var isHit = Physics.CapsuleCast(point1, point2, capsuleRadius, direction, out var hitInfo, maxDistance, layerMask);

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * hitInfo.distance);
                Gizmos.DrawSphere(hitInfo.point, 0.05f);
                Gizmos.DrawWireMesh(capsuleMesh, origin + direction * hitInfo.distance, transform.rotation, CapsuleSize);
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxDistance);
                Gizmos.DrawWireMesh(capsuleMesh, origin + direction * maxDistance, transform.rotation, CapsuleSize);
            }
        }

        private void SphereCastAll(Vector3 point1, Vector3 point2, Vector3 direction)
        {
            var origin = Vector3.Lerp(point1, point2, 0.5f);
            var hitInfos = Physics.CapsuleCastAll(point1, point2, capsuleRadius, direction, maxDistance, layerMask);

            var isHit = hitInfos.Length > 0;

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * maxDistance);

                foreach (var hitInfo in hitInfos)
                {
                    Gizmos.DrawSphere(hitInfo.point, 0.05f);
                    Gizmos.DrawWireMesh(capsuleMesh, origin + direction * hitInfo.distance, transform.rotation, CapsuleSize);
                }
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxDistance);
                Gizmos.DrawWireMesh(capsuleMesh, origin + direction * maxDistance, transform.rotation, CapsuleSize);
            }
        }

        private void SphereCastAllDoubleSided(Vector3 point1, Vector3 point2, Vector3 direction)
        {
            var origin = Vector3.Lerp(point1, point2, 0.5f);
            SphereCastAll(point1, point2, direction);
            SphereCastAll(origin + direction * maxDistance + point1, origin + direction * maxDistance + point2, -direction);
        }


        private void OnDrawGizmos()
        {
            var origin = transform.position;
            var direction = transform.forward;
            var rotation = transform.rotation;
            var point1 = origin + rotation * (Vector3.up * capsuleHeight * 0.5f);
            var point2 = origin + rotation * (Vector3.down * capsuleHeight * 0.5f);

            switch (raycastType)
            {
                case CapsuleCastType.SphereCast:
                    SphereCast(point1, point2, direction);
                    break;
                
                case CapsuleCastType.SphereCastAll:
                    SphereCastAll(point1, point2, direction);
                    break;
                
                case CapsuleCastType.SphereCastAllDoubleSided:
                    SphereCastAllDoubleSided(point1, point2, direction);
                    break;
            }
        }
    }
}
