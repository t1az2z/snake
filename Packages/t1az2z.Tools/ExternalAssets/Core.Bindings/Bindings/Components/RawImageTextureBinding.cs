using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(RawImage))]
    public class RawImageTextureBinding : BaseBinding<TextureProperty> {
        private RawImage _component;
        private bool _isBlank;

        protected override void Awake() {
            _component = GetComponent<RawImage>();
            _isBlank = false;
            base.Awake();
        }

        protected override void OnValueUpdated() {
            var texture = Property.Value;
            if (texture == null) {
                if (!_isBlank) {
                    texture = CreateBlank();

                    _isBlank = true;
                }
            }
            else if (_isBlank) {
                Destroy(_component.texture);

                _isBlank = false;
            }

            _component.texture = texture;
        }

        private Texture2D CreateBlank() {
            var textureBlank = new Texture2D(1, 1);
            textureBlank.SetPixel(0, 0, new Color(0, 0, 0, 0));
            textureBlank.Apply();
            return textureBlank;
        }
    }
}