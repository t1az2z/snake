using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(RawImage))]
    public class RawImageMaterialBinding : BaseBinding<MaterialProperty> {
        private RawImage _component;

        protected override void Awake() {
            _component = GetComponent<RawImage>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _component.material = Property.Value;
        }
    }
}