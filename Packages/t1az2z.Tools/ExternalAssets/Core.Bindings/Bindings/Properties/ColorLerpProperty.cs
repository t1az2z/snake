using Binding;
using UnityEngine;

namespace Core.Core.Bindings.Bindings.Properties {
    public class ColorLerpProperty : ColorProperty {
        public float LerpTime { get; set; }
        public bool Force { get; private set; }

        public ColorLerpProperty() : base() {
            LerpTime = 1f;
        }

        public ColorLerpProperty(float startLerpTime = 1f) : base() {
            LerpTime = startLerpTime;
        }

        public ColorLerpProperty(Color startColor, float startLerpTime = 1f) : base(startColor) {
            LerpTime = startLerpTime;
        }

        public void SetForce(Color color) {
            Force = true;
            Value = color;
            Force = false;
        }
    }
}