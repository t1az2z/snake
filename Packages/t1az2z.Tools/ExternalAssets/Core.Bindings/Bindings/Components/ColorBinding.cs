using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Graphic))]
    public class ColorBinding : BaseBinding<ColorProperty> {
        private Graphic _graphic;

        protected override void Awake() {
            _graphic = GetComponent<Graphic>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _graphic.color = Property.Value;
        }
    }
}