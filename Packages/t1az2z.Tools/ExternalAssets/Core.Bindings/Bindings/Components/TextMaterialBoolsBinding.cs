using Binding;
using Binding.Base;
using TMPro;
using UnityEngine;

namespace Core.Bindings.Components {
    public class TextMaterialBoolsBinding : BaseMultiBinding {
        [SerializeField] private Material[] TrueMaterials = default;
        [SerializeField] private Material AllFalseMaterial = default;
        
        private TextMeshProUGUI _component;

        protected override void Awake() {
            _component = GetComponent<TextMeshProUGUI>();
            base.Awake();
        }

        protected override void OnValueCaptured() {
            for (int i = 0; i < properties.Length; ++i) {
                if ((properties[i] is BoolProperty bp) && bp.Value) {
                    _component.enabled = true;
                    _component.fontSharedMaterial = TrueMaterials[i];
                    return;
                }
            }

            CheckAllFalse();
        }

        protected override void OnValueChanged(IProperty property, int index) {
            if ((property is BoolProperty bp) && bp.Value) {
                _component.enabled = true;
                _component.fontSharedMaterial = TrueMaterials[index];
            }
            else {
                CheckAllFalse();
            }
        }

        private void CheckAllFalse() {
            foreach (BoolProperty itr in properties) {
                if (itr.Value) {
                    return;
                }
            }

            if (AllFalseMaterial) {
                _component.enabled = true;
                _component.fontSharedMaterial = AllFalseMaterial;
            }
            else {
                _component.enabled = false;
            }
        }
    }
}