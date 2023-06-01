using Binding.Base;
using UnityEngine;

namespace Binding {
    public class MaterialProperty : Property<Material> {
        public MaterialProperty() : base() { }

        public MaterialProperty(Material startValue = null) : base(startValue) { }
    }
}