using UnityEngine;

namespace t1az2z.Tools.Tools.Common
{
    public class LookAtCameraObject : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}