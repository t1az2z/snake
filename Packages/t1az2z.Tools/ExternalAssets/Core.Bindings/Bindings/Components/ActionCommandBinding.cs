using Binding;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Bindings.Components {
    public class ActionCommandBinding : BaseBinding<CommandProperty> {
        [SerializeField] private UnityEvent Action = null;

        protected override void OnValueUpdated() {
            Action?.Invoke();
        }
    }
}