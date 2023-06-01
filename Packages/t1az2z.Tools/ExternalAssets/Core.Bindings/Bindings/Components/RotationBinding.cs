using Binding;

namespace Core.Bindings.Components {
    public class RotationBinding : BaseBinding<QuaternionProperty> {
        protected override void OnValueUpdated() {
            transform.rotation = Property.Value;
        }
    }
}