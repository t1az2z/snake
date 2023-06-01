using Binding.Base;
using UnityEngine;

namespace Binding {
    public static class ColorPropertyColorSetters {
        public static void SetAlpha(this ColorProperty property, float alpha) {
            var color = property.Value;
            color.a = alpha;
            property.Value = color;
        }

        public static void SetColorOnly(this ColorProperty property, Color color) {
            color.a = property.Value.a;
            property.Value = color;
        }

        public static void SetOpaqueColor(this ColorProperty property, Color color) {
            color.a = 1f;
            property.Value = color;
        }
    }

    public class ColorProperty : Property<Color> {
        public ColorProperty() : base() { }

        public ColorProperty(Color startValue = default) : base(startValue) { }
    }
}