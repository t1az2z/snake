using System;
using Binding;
using UnityEngine;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererColorIntBinding : BaseBinding<IntProperty> {
        [SerializeField] private ColorElement[] _elements = null;

        /// <summary>
        /// Если значение property.value вышло за пределы массива цветов,
        /// то использовать его граничные цвета
        /// </summary>
        [SerializeField] private bool _useBoundaryValues = false;

        private SpriteRenderer _renderer;

        protected override void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            if (_elements == null || _elements.Length == 0) {
                return;
            }

            var value = Property.Value;

            if (_useBoundaryValues) {
                if (value < _elements[0].Value) {
                    _renderer.color = _elements[0].Color;

                    return;
                }
                else if (value > _elements[_elements.Length - 1].Value) {
                    _renderer.color = _elements[_elements.Length - 1].Color;

                    return;
                }
            }

            for (int i = 0; i < _elements.Length; i++) {
                var element = _elements[i];
                if (element.Value == value) {
                    _renderer.color = element.Color;
                    break;
                }
            }
        }
        
        [Serializable]
        public class ColorElement {
            public int Value => _value;

            public Color Color => _color;

            [SerializeField] private int _value = 0;
            [SerializeField] private Color _color = Color.white;
        }
    }
}