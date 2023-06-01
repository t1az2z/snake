using Binding.Base;
using System;
using UnityEngine;

namespace Binding {
    public class ColorStrStr : BaseBindingTarget, IEquatable<ColorStrStr> {
        public Color Color { get; set; }
        public string Str1 { get; set; }
        public string Str2 { get; set; }

        public bool Equals(ColorStrStr other) {
            bool result = false;
            result |= Color != other.Color;
            result |= Str1 != other.Str1;
            result |= Str2 != other.Str2;
            return result;
        }
    }

    public class ColorStrStrProperty : Property<ColorStrStr> {
        public ColorStrStrProperty() : base() { }

        public ColorStrStrProperty(ColorStrStr startValue) : base(startValue) { }
    }
}