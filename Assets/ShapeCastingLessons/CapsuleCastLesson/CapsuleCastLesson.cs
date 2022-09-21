using UnityEditor;
using UnityEngine;

namespace CapsuleCastLesson
{
    public class CapsuleCastLesson : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField, Min(0f)] private float capsuleHeight;
        [SerializeField, Min(0f)] private float capsuleRadius;
        [SerializeField, Min(0f)] private float maxDistance;
        [SerializeField] private CapsuleCastType raycastType;


        private enum CapsuleCastType
        {
            CapsuleCast,
            CapsuleCastAll,
            CapsuleCastAllDoubleSided,
        }


        private float CapsuleHeight => Mathf.Max(capsuleRadius * 2f, capsuleHeight);


        private void SphereCast(Vector3 point1, Vector3 point2, Vector3 direction)
        {
            var origin = Vector3.Lerp(point1, point2, 0.5f);
            var isHit = Physics.CapsuleCast(point1, point2, capsuleRadius, direction, out var hitInfo, maxDistance, layerMask);

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * hitInfo.distance);
                Gizmos.DrawSphere(hitInfo.point, 0.05f);
                DrawWireCapsule(origin + direction * hitInfo.distance, Gizmos.color);
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxDistance);
                DrawWireCapsule(origin + direction * maxDistance, Gizmos.color);
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
                    DrawWireCapsule(origin + direction * hitInfo.distance, Gizmos.color);
                }
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxDistance);
                DrawWireCapsule(origin + direction * maxDistance, Gizmos.color);
            }
        }

        private void SphereCastAllDoubleSided(Vector3 point1, Vector3 point2, Vector3 direction)
        {
            var origin = Vector3.Lerp(point1, point2, 0.5f);
            SphereCastAll(point1, point2, direction);
            SphereCastAll(origin + direction * maxDistance + point1, origin + direction * maxDistance + point2, -direction);
        }


        private void DrawWireCapsule(Vector3 position, Color color = default)
        {
            if (color != default)
            {
                Handles.color = color;
            }
            
            var angleMatrix = Matrix4x4.TRS(position, transform.rotation, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (CapsuleHeight - (capsuleRadius * 2)) / 2;
                
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, capsuleRadius);
                Handles.DrawLine(new Vector3(0, pointOffset, -capsuleRadius), new Vector3(0, -pointOffset, -capsuleRadius));
                Handles.DrawLine(new Vector3(0, pointOffset, capsuleRadius), new Vector3(0, -pointOffset, capsuleRadius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, capsuleRadius);
                
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, capsuleRadius);
                Handles.DrawLine(new Vector3(-capsuleRadius, pointOffset, 0), new Vector3(-capsuleRadius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(capsuleRadius, pointOffset, 0), new Vector3(capsuleRadius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, capsuleRadius);
                
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, capsuleRadius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, capsuleRadius);
            }
        }
        
        private void OnDrawGizmos()
        {
            var origin = transform.position;
            var direction = transform.forward;
            var rotation = transform.rotation;
            var point1 = origin + rotation * (Vector3.up * CapsuleHeight * 0.25f);
            var point2 = origin + rotation * (Vector3.down * CapsuleHeight * 0.25f);

            switch (raycastType)
            {
                case CapsuleCastType.CapsuleCast:
                    SphereCast(point1, point2, direction);
                    break;
                
                case CapsuleCastType.CapsuleCastAll:
                    SphereCastAll(point1, point2, direction);
                    break;
                
                case CapsuleCastType.CapsuleCastAllDoubleSided:
                    SphereCastAllDoubleSided(point1, point2, direction);
                    break;
            }
        }
    }
}
