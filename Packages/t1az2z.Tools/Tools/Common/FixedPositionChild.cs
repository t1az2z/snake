using UnityEngine;

namespace t1az2z.Tools.Tools.Common
{
    [ExecuteInEditMode]
    public class FixedPositionChild : MonoBehaviour
    {
#if UNITY_EDITOR
        private Vector3 _prevPosition;
        private Vector3 _parentPosition;
        private void Update()
        {
            if (Application.isPlaying)
                return;
            
            if (_parentPosition != transform.parent.position)
            {
                _parentPosition = transform.parent.position;
                transform.position = _prevPosition;
            }
            else
            {
                _prevPosition = transform.position;
            }
        }
#endif
    }
}