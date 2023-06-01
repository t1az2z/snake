using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    public class RotationAxisBinding : BaseBinding<FloatProperty> {
        [SerializeField] private AxisTypeEnum Axis = AxisTypeEnum.X;
        [SerializeField] private bool Local = false;
        [SerializeField] private bool Invert = false;

        protected override void OnValueUpdated() {
            var eulerAngles = transform.eulerAngles;
            var angle = Invert ? -Property.Value : Property.Value;
            switch (Axis) {
                case AxisTypeEnum.X:
                    SetAngle(angle, eulerAngles.y, eulerAngles.z);
                    break;

                case AxisTypeEnum.Y:
                    SetAngle(eulerAngles.x, angle, eulerAngles.z);
                    break;

                case AxisTypeEnum.Z:
                    SetAngle(eulerAngles.x, eulerAngles.y, angle);
                    break;
            }
        }

        private void SetAngle(float x, float y, float z) {
            var angle = new Vector3(x, y, z);
            if (Local) {
                transform.localEulerAngles = angle;
            }
            else {
                transform.eulerAngles = angle;
            }
        }

        private enum AxisTypeEnum {
            X,
            Y,
            Z
        }
    }
}