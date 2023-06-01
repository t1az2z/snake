using Binding;
using Binding.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Slider))]
    public class SliderMaxBinding : BaseBinding<IProperty> {
        private Slider _slider;

        protected override void Awake() {
            _slider = GetComponent<Slider>();
            base.Awake();
            OnValueUpdated();
        }

        protected override void OnValueUpdated() {
            switch (Property) {
                case IntProperty intProperty:
                    _slider.maxValue = intProperty.Value;
                    break;

                case FloatProperty floatProperty:
                    _slider.maxValue = floatProperty.Value;
                    break;
            }
        }
    }
}