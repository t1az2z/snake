using Binding.Base;
using UnityEngine;

namespace Binding {
    public class Vector2Property : Property<Vector2> {
        public Vector2Property() : base() { }

        public Vector2Property(Vector2 startValue = default) : base(startValue) { }
    }
}