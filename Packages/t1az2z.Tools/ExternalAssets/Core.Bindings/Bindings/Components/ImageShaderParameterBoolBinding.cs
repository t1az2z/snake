using Core.Bindings.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class ImageShaderParameterBoolBinding : BoolBinding {
        [SerializeField] private string Parameter;
        [SerializeField] private ParameterTypeEnum Type;
        [SerializeField] private string TrueValue = default;
        [SerializeField] private string FalseValue = default;

        private Image _component;
        private int _parameterID;

        protected override void Awake() {
            _component = GetComponent<Image>();
            _component.material = Instantiate(_component.material);
            _parameterID = Shader.PropertyToID(Parameter);
            base.Awake();
        }

        protected override void OnValueUpdated() {
            var value = new ConvertableString( BoolValue ? TrueValue : FalseValue);
            _component.enabled = _component.sprite != null;
            switch (Type) {
                case ParameterTypeEnum.Int:
                    _component.materialForRendering.SetInt(_parameterID, value);
                    break;

                case ParameterTypeEnum.Float:
                    _component.materialForRendering.SetFloat(_parameterID, value);
                    break;
            }
        }

        public enum ParameterTypeEnum {
            Float,
            Int,
        }
    }
}