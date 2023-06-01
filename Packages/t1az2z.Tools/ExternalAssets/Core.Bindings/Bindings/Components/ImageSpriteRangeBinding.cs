using System;
using Binding;
using Binding.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Image))]
    public class ImageSpriteRangeBinding : BaseBinding<IProperty> {
        [SerializeField] private RangeNode[] Ranges = default;
        [SerializeField] public Sprite DefaultSprite;

        private Image _component;
        private RangeNode _activeRange;

        protected override void Awake() {
            _component = GetComponent<Image>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            int curVal = 0;
            switch (Property) {
                case IntProperty ip:
                    curVal = ip.Value;
                    break;

                case FloatProperty ip:
                    curVal = (int)ip.Value;
                    break;
            }

            if (_activeRange?.InRange(curVal) == true) {
                return;
            }

            foreach (var itr in Ranges) {
                if (itr.InRange(curVal)) {
                    _activeRange = itr;
                    if (itr.Sprite == null) {
                        _component.enabled = false;
                    }
                    else {
                        _component.enabled = true;
                        _component.sprite = itr.Sprite;
                    }
                    return;
                }
            }

            _component.enabled = DefaultSprite != null;
            _component.sprite = DefaultSprite;
            _activeRange = null;
        }

        [Serializable]
        public class RangeNode {
            public Sprite Sprite;
            public int FromValue;
            public int ToValue;

            public bool InRange(int curVal) {
                return (curVal >= FromValue) && (curVal <= ToValue);
            }
        }
    }
}