using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Graphic))]
    public class GraphicMaterialBoolBinding : BoolBinding {
        [SerializeField] private Material MaterialOnTrue;
        [SerializeField] private Material MaterialOnFalse;

        private Graphic _graphic;

        protected override void Awake() {
            _graphic = GetComponent<Graphic>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            if (BoolValue) {
                _graphic.material = MaterialOnTrue;
            }
            else {
                _graphic.material = MaterialOnFalse;
            }
        }
    }
}