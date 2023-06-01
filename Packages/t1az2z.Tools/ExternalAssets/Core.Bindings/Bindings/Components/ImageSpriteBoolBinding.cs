using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Image))]
    public class ImageSpriteBoolBinding : BoolBinding {
        [SerializeField] private Sprite TrueSprite = default;
        [SerializeField] private Sprite FalseSprite = default;
        [SerializeField] private bool UseNativeSize = default;
        
        private Image _component;

        protected override void Awake() {
            _component = GetComponent<Image>();    
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _component.sprite = BoolValue ? TrueSprite : FalseSprite;
            _component.enabled = _component.sprite != null;
            if (UseNativeSize) {
                _component.SetNativeSize();
            }
        }
    }
}