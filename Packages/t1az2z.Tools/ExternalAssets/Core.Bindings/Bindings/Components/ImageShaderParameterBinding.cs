using Binding;
using Binding.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class ImageShaderParameterBinding : BaseBinding<IProperty> {
        [SerializeField] private string Parameter;
        private Image _component;
        private int _parameterID;

        protected override void Awake() {
            _component = GetComponent<Image>();
            _component.material = Instantiate(_component.material);
            _parameterID = Shader.PropertyToID(Parameter);
            base.Awake();
        }

        protected override void OnValueUpdated() {
            switch (Property) {
                case FloatProperty floatProperty:
                    _component.materialForRendering.SetFloat(_parameterID, floatProperty.Value);
                    break;
                default:
                    Debug.LogError($"{nameof(ImageShaderParameterBinding)}> Unsupported property of type {Property.GetType()}");
                    break;
            }
        }
    }
}