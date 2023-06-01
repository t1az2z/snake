using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Graphic))]
    public class ColorChannelBinding : BaseBinding<FloatProperty> {
        [SerializeField] private ChannelType Channel = ChannelType.Red;
        
        private Graphic _graphic;

        protected override void Awake() {
            _graphic = GetComponent<Graphic>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            var color = _graphic.color;
            switch (Channel) {
                case ChannelType.Red:
                    color.r = Property.Value;
                    break;

                case ChannelType.Green:
                    color.g = Property.Value;
                    break;

                case ChannelType.Blue:
                    color.b = Property.Value;
                    break;

                case ChannelType.Alpha:
                    color.a = Property.Value;
                    break;
            }

            _graphic.color = color;
        }
        
        private enum ChannelType {
            Red,
            Green,
            Blue,
            Alpha
        }
    }
}