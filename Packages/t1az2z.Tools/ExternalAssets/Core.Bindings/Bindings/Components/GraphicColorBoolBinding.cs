using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Graphic))]
    public class GraphicColorBoolBinding : BoolBinding {
        [SerializeField] private Color ColorOnTrue = Color.white;
        [SerializeField] private Color ColorOnFalse = Color.white;

        private Graphic _graphic;

        protected override void Awake() {
            _graphic = GetComponent<Graphic>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            if (BoolValue) {
                _graphic.color = ColorOnTrue;
            }
            else {
                _graphic.color = ColorOnFalse;
            }
        }
    }
}