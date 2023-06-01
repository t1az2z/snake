using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Image))]
    public class AtlasSpriteBinding : BaseBinding {
        [SerializeField] private SpriteAtlas[] Atlases = default;
        [SerializeField] private Sprite Default = default;
        
        private Image _component;

        protected override void Awake() {
            _component = GetComponent<Image>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            Sprite sprite = null;
            for (int i = 0; i < Atlases.Length; i++) {
                var atlas = Atlases[i];
                if (atlas != null) {
                    sprite = atlas.GetSprite(Property.ToString());
                    if (sprite != null) {
                        break;
                    }
                }
            }

            _component.sprite = sprite ?? Default;
        }
    }
}