using Binding.Base;
using UnityEngine;

namespace Binding {
    public class RectProperty : Property<Rect> {
        public RectProperty() : base() { }

        public RectProperty(Rect startValue = default) : base(startValue) { }
    }
}