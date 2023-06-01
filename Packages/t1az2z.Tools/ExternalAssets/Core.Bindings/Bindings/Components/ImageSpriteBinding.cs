using Binding;
using Core.Bindings.Tools.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Image))]
    public class ImageSpriteBinding : BaseBinding<SpriteProperty> {
        [SerializeField] private Sprite DefaultSprite = default;
        [SerializeField] private bool SetNativeSize = default;
        [SerializeField] private bool OffsetBySpritePivot = default;


        private Image _component;
        private bool _initialFlipPositive;
        private Vector3 _origin;

        protected override void Awake() {
            _component = GetComponent<Image>();
            _origin = _component.rectTransform.anchoredPosition;
            _initialFlipPositive = _component.transform.localScale.x > 0;
            base.Awake();
        }

        protected override void OnValueUpdated() {
            var oldSprite = _component.sprite;
            _component.sprite = Property.Value ?? DefaultSprite;
            var changed = oldSprite != _component.sprite;

            _component.enabled = (_component.sprite != null);
            if (changed && SetNativeSize) {
                _component.SetNativeSize();
            }

            var x = _component.transform.localScale.x;
            var sign = x > 0;
            var shouldReverse = Property.FlipX && (_initialFlipPositive == sign);
            shouldReverse |= !Property.FlipX && (_initialFlipPositive != sign);
            if (shouldReverse) {
                _component.transform.localScale = _component.transform.localScale.WithX(-x);
            }

            if (OffsetBySpritePivot) {
                var size = _component.rectTransform.sizeDelta;
                size *= _component.pixelsPerUnit;
                var pixelPivot = _component.sprite.pivot;
                var percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
                _component.rectTransform.pivot = percentPivot;
                _component.rectTransform.anchoredPosition = _origin;
            }
        }
    }
}