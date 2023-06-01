using Binding;
using UnityEngine;
using UnityEngine.U2D;

namespace Core.Bindings.Components {
    public class VisibilityAtlasBinding : BaseBinding<StringProperty> {
        [SerializeField] private SpriteAtlas[] Atlases;
        [SerializeField] private bool OnSpriteExist = false;

        protected override void OnValueUpdated() {
            for (int i = 0; i < Atlases.Length; i++) {
                var atlas = Atlases[i];
                if (atlas == null)
                    continue;

                var sprite = atlas.GetSprite(Property.Value);
                if (sprite == null)
                    continue;

                gameObject.SetActive(OnSpriteExist);
                return;
            }

            gameObject.SetActive(!OnSpriteExist);
        }
    }
}