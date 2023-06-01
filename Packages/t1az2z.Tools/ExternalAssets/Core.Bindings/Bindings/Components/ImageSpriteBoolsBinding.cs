using Binding;
using Binding.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Image))]
    public class ImageSpriteBoolsBinding : BaseMultiBinding {
        [SerializeField] private Sprite[] TrueSprites = default;
        [SerializeField] private Sprite AllFalseSprite = default;
        
        private Image _component;

        protected override void Awake() {
            _component = GetComponent<Image>();
            base.Awake();
        }

        protected override void OnValueCaptured() {
            for (int i = 0; i < properties.Length; ++i) {
                if ((properties[i] is BoolProperty bp) && bp.Value) {
                    _component.enabled = true;
                    _component.sprite = TrueSprites[i];
                    return;
                }
            }

            CheckAllFalse();
        }

        protected override void OnValueChanged(IProperty property, int index) {
            if ((property is BoolProperty bp) && bp.Value) {
                _component.enabled = true;
                _component.sprite = TrueSprites[index];
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

            if (AllFalseSprite) {
                _component.enabled = true;
                _component.sprite = AllFalseSprite;
            }
            else {
                _component.enabled = false;
            }
        }
    }
}