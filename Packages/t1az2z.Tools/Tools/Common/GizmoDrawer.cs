using UnityEngine;

namespace t1az2z.Tools.Tools.Common
{
    public class GizmoDrawer : MonoBehaviour
    {
        [SerializeField] private BoxCollider Collider;
        [SerializeField] private Color Color = Color.white;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Collider == null)
                Collider = GetComponent<BoxCollider>();
        }
#endif

        private void OnDrawGizmos()
        {
            Gizmos.color = Color;
            if (Collider == null)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
            }
            else
            {
                if (!Collider.enabled)
                    return;
                
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(Collider.center, Collider.size);
            }
        }
    }
}