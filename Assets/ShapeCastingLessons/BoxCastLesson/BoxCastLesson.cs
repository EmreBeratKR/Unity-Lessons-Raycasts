using System;
using UnityEngine;

namespace BoxCastLesson
{
    public class BoxCastLesson : MonoBehaviour
    {
        [SerializeField] private Mesh cubeMesh;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Vector3 boxSize;
        [SerializeField, Min(0f)] private float maxDistance;
        [SerializeField] private BoxCastType raycastType;


        private enum BoxCastType
        {
            BoxCast,
            BoxCastAll,
            BoxCastAllDoubleSided,
        }


        private void BoxCast(Vector3 origin, Vector3 direction, Quaternion rotation)
        {
            var isHit = Physics.BoxCast(origin,
                boxSize * 0.5f, 
                direction, 
                out var hitInfo,
                rotation,
                maxDistance, 
                layerMask);

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * hitInfo.distance);
                Gizmos.DrawSphere(hitInfo.point, 0.05f);
                Gizmos.DrawWireMesh(cubeMesh, origin + direction * hitInfo.distance, rotation, boxSize);
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxDistance);
                Gizmos.DrawWireMesh(cubeMesh, origin + direction * maxDistance, rotation, boxSize);
            }
        }

        private void BoxCastAll(Vector3 origin, Vector3 direction, Quaternion rotation)
        {
            var hitInfos = Physics.BoxCastAll(origin,
                boxSize * 0.5f,
                direction,
                rotation,
                maxDistance,
                layerMask);

            var isHit = hitInfos.Length > 0;

            if (isHit)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(origin, direction * maxDistance);

                foreach (var hitInfo in hitInfos)
                {
                    Gizmos.DrawSphere(hitInfo.point, 0.05f);
                    Gizmos.DrawWireMesh(cubeMesh, origin + direction * hitInfo.distance, rotation, boxSize);
                }
            }

            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(origin, direction * maxDistance);
                Gizmos.DrawWireMesh(cubeMesh, origin + direction * maxDistance, rotation, boxSize);
            }
        }

        private void BoxCastAllDoubleSided(Vector3 origin, Vector3 direction, Quaternion rotation)
        {
            BoxCastAll(origin, direction, rotation);
            BoxCastAll(origin + direction * maxDistance, -direction, rotation);
        }


        private void OnDrawGizmos()
        {
            var origin = transform.position;
            var direction = transform.forward;
            var rotation = transform.rotation;
            
            switch (raycastType)
            {
                case BoxCastType.BoxCast:
                    BoxCast(origin, direction, rotation);
                    break;
                
                case BoxCastType.BoxCastAll:
                    BoxCastAll(origin, direction, rotation);
                    break;
                
                case BoxCastType.BoxCastAllDoubleSided:
                    BoxCastAllDoubleSided(origin, direction, rotation);
                    break;
            }
        }

        private void OnValidate()
        {
            boxSize.x = Mathf.Max(0f, boxSize.x);
            boxSize.y = Mathf.Max(0f, boxSize.y);
            boxSize.z = Mathf.Max(0f, boxSize.z);
        }
    }
}
