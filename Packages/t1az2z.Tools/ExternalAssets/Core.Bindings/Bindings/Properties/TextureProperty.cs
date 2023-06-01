using Binding.Base;
using UnityEngine;

namespace Binding {
    public class TextureProperty : Property<Texture> {
        public TextureProperty() : base() { }

        public TextureProperty(Texture startValue = null) : base(startValue) { }
    }
}