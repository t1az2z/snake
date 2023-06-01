using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class InteractableBinding : BoolBinding {
        private Selectable _component;

        protected override void Awake() {
            _component = GetComponent<Selectable>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _component.interactable = BoolValue;
        }
    }
}