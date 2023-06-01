using Binding.Base;

namespace Binding {
    public class LongProperty : Property<long> {
        public LongProperty() : base() { }

        public LongProperty(int startValue = 0) : base(startValue) { }
    }
}