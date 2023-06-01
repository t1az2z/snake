using UnityEngine;

namespace t1az2z.Tools.Tools.Common
{
    public class FixedPositionOffset : MonoBehaviour
    {
        [SerializeField] private Transform Target;
        [SerializeField] private Vector3 Offset;

        private void Update()
        {
            transform.position = Target.position + Offset;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Target == null)
                return;
            
            transform.position = Target.position + Offset;
        }
#endif
    }
}