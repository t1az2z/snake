using Binding.Base;

namespace Binding {
    public class IntProperty : Property<int> {
        public IntProperty() : base() { }

        public IntProperty(int startValue = 0) : base(startValue) { }
    }
}