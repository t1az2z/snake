using System;
using Binding;
using TMPro;
using UnityEngine;

namespace Core.Bindings.Components {
    public class TextMaterialsIntBinding : BaseBinding<IntProperty> {
        [SerializeField] private MaterialElement[] Elements;
        [SerializeField] private Material Default = null;
        [SerializeField] private bool UseBoundaryValues = false;

        private TMP_Text _text;

        protected override void Awake() {
            _text = GetComponent<TMP_Text>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            if (Elements == null || Elements.Length == 0) {
                return;
            }

            var value = Property.Value;

            if (value < Elements[0].Value) {
                _text.fontSharedMaterial = UseBoundaryValues ? Elements[0].Material : Default;
                return;
            }
            
            if (value > Elements[Elements.Length - 1].Value) {
                _text.fontSharedMaterial = UseBoundaryValues ? Elements[Elements.Length - 1].Material : Default;

                return;
            }

            for (int i = 0; i < Elements.Length; i++) {
                var element = Elements[i];
                if (element.Value == value) {
                    _text.fontSharedMaterial = element.Material;
                    break;
                }
            }
        }
        
        [Serializable]
        public class MaterialElement {
            public int Value => _value;

            public Material Material => _material;

            [SerializeField] private int _value = 0;
            [SerializeField] private Material _material;
        }
    }
}