using Binding;

namespace Core.Bindings.Components {
    public class TransformBinding : BaseBinding<TransformProperty> {
        protected override void Awake() {
            base.Awake();
            Property.Value = transform;
        }
    }
}