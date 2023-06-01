using Binding.Base;
using UnityEngine;

namespace Binding {
    public class QuaternionProperty : Property<Quaternion> {
        public QuaternionProperty() : base() { }

        public QuaternionProperty(Quaternion startValue = default) : base(startValue) { }
    }
}