using System;

namespace Binding {
    public class AlphaFadeProperty : FloatProperty {
        public event Action OnFadeFinish;

        public float FadeTime { get; set; }
        public bool Force { get; private set; }

        public AlphaFadeProperty() : base() {
            FadeTime = 1f;
        }

        public AlphaFadeProperty(float startFadeTime = 1f) : base() {
            FadeTime = startFadeTime;
        }

        public AlphaFadeProperty(float startValue = 0f, float startFadeTime = 1f) : base(startValue) {
            FadeTime = startFadeTime;
        }

        public void SetForce(float alpha) {
            Force = true;
            Value = alpha;
            Force = false;
        }

        public void Finish() {
            OnFadeFinish?.Invoke();
        }
    }
}