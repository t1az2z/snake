using Binding.Base;

namespace Binding {
    public class FloatProperty : Property<float> {
        public FloatProperty() : base() { }

        public FloatProperty(float startValue = 0f) : base(startValue) { }
    }
}