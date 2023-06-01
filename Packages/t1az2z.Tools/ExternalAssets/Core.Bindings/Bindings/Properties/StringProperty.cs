using Binding.Base;

namespace Binding {
    public class StringProperty : Property<string> {
        public StringProperty() : base() { }

        public StringProperty(string startValue = null) : base(startValue) { }
    }
}