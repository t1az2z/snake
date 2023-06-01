using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class CommandBinding : BaseBinding<CommandProperty> {
        public void Invoke() => Property?.Invoke();
    }
}