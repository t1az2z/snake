using Binding.Base;

namespace Binding {
    public class BoolProperty : Property<bool> {
        public BoolProperty() : base() { }

        public BoolProperty(bool startValue = false) : base(startValue) { }
    }
}