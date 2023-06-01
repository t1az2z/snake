using UnityEngine;

namespace t1az2z.Tools.Tools.Common
{
    public class FixRotation : MonoBehaviour
    {
        [SerializeField] private Vector3 Rotation;

        private void Update()
        {
            if (transform.rotation.eulerAngles != Rotation)
                transform.rotation = Quaternion.Euler(Rotation);
        }

#if UNITY_EDITOR
        [ContextMenu("Copy Rotation")]
        public void CopyRotation()
        {
            Rotation = transform.rotation.eulerAngles;
        }
#endif
    }
}