using UnityEngine;

namespace Core
{
    public class TimeService : MonoBehaviour
    {
        public float TimeScale = 1f;
        public float DeltaTime { get; private set; }
        public float FixedDeltaTime{ get; private set; }

        private void Update()
        {
            DeltaTime = Time.deltaTime * TimeScale;
        }

        private void FixedUpdate()
        {
            FixedDeltaTime = Time.fixedDeltaTime * TimeScale;
        }
    }
}