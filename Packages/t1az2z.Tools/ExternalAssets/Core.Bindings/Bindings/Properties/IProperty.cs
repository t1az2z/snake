using System;

namespace Binding.Base {
    public interface IEditorNotAllowedDropdownAsIProperty {}
    public interface ICommand {}

    public interface IProperty {
        event Action OnValueChanged;
        IDisposable SubscribeOnValueChanged(Action action);

        string InstanceName { get; set; }

        string GetFormattedString(string arg);
        string FormatString { get; set; }
        string ToString();
        object ToObject();
        bool IsEmpty();
    }
}