using UnityEngine;

namespace RaycastLesson
{
    public class RaycastLesson : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField, Min(0f)] private float maxRayDistance;
        [SerializeField] private RaycastType raycastType;


        private enum RaycastType
        {
            Raycast,
            RaycastAll,
            RaycastAllDoubleSided
        }
        

        private void Raycast(Vector3 origin, Vector3 direction)
        {
            var isHit = Physics.Raycast(origin, direction, out var hitInfo, maxRayDistance, layerMask);

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * hitInfo.distance);
                Gizmos.DrawSphere(hitInfo.point, 0.05f);
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxRayDistance);
            }
        }

        private void RaycastAll(Vector3 origin, Vector3 direction)
        {
            var hitInfos = Physics.RaycastAll(origin, direction, maxRayDistance, layerMask);
            var isHit = hitInfos.Length > 0;

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * maxRayDistance);
                foreach (var hitInfo in hitInfos)
                {
                    Gizmos.DrawSphere(hitInfo.point, 0.05f);
                }
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxRayDistance);
            }
        }

        private void RaycastAllDoubleSided(Vector3 origin, Vector3 direction)
        {
            RaycastAll(origin, direction);
            RaycastAll(origin + direction * maxRayDistance, -direction);
        }
    
        private void OnDrawGizmos()
        {
            var origin = transform.position;
            var direction = transform.forward;
        
        
            switch (raycastType)
            {
                case RaycastType.Raycast:
                    Raycast(origin, direction);
                    break;
            
                case RaycastType.RaycastAll:
                    RaycastAll(origin, direction);
                    break;
                
                case RaycastType.RaycastAllDoubleSided:
                    RaycastAllDoubleSided(origin, direction);
                    break;
            }
        }
    }
}
