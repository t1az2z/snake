using Binding.Base;
using UnityEngine;

namespace Binding {
    public class SpriteProperty : Property<Sprite> {
        private bool _flipX = false;
        public bool FlipX { get => _flipX;
        set {
                if (_flipX != value) {
                    _flipX = value;
                    ValueChanged();
                }
            }
        }

        public SpriteProperty() : base() { }

        public SpriteProperty(Sprite startValue = null) : base(startValue) { }
    }
}