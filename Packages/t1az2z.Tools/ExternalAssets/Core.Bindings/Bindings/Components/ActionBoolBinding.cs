using UnityEngine;
using UnityEngine.Events;

namespace Core.Bindings.Components {
    public class ActionBoolBinding : BoolBinding {
        [SerializeField] private UnityEvent TrueAction = null;
        [SerializeField] private UnityEvent FalseAction = null;

        protected override void OnValueUpdated() {
            if (BoolValue) {
                TrueAction?.Invoke();
            }
            else {
                FalseAction?.Invoke();
            }
        }
    }
}