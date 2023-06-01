using System;
using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class SpriteIntBinding : BaseBinding<IntProperty> {
        [SerializeField] private SpriteElement[] _elements = null;
        [SerializeField] private Sprite _default = null;
        [SerializeField] private bool _useBoundaryValues = false;

        private Image _image;
        private SpriteRenderer _renderer;

        protected override void Awake() {
            _image = GetComponent<Image>();
            _renderer = GetComponent<SpriteRenderer>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            if (_elements == null || _elements.Length == 0) {
                return;
            }

            var value = Property.Value;

            if (value < _elements[0].Value) {
                SetSprite(_useBoundaryValues ? _elements[0].Sprite : _default);
                return;
            }
            else if (value > _elements[_elements.Length - 1].Value) {
                SetSprite(_useBoundaryValues ? _elements[_elements.Length - 1].Sprite : _default);

                return;
            }

            for (int i = 0; i < _elements.Length; i++) {
                var element = _elements[i];
                if (element.Value == value) {
                    SetSprite(element.Sprite);
                    break;
                }
            }
        }

        private void SetSprite(Sprite sprite) {
            if (_image != null) _image.sprite = sprite;
            if (_renderer != null) _renderer.sprite = sprite;
        }
        
        [Serializable]
        public class SpriteElement {
            public int Value => _value;

            public Sprite Sprite => _sprite;

            [SerializeField] private int _value = 0;
            [SerializeField] private Sprite _sprite;
        }
    }
}