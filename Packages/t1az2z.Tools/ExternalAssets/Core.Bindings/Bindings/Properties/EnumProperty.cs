using Binding.Base;
using System;

namespace Binding {
    public class EnumProperty : Property<Enum> {
        public EnumProperty() : base() { }

        public EnumProperty(Enum startValue = null) : base(startValue) { }
    }
}