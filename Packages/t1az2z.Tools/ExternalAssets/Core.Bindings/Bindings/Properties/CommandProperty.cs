using Binding.Base;
using System;
using Core.Bindings.Tools.Rx;

namespace Binding {
    public abstract class CommandProperty<T> : IProperty, ICommand, IEditorNotAllowedDropdownAsIProperty, IObservable<T> {
        string IProperty.InstanceName {
            get { return _instanceName; }
            set { _instanceName = value; }
        }

        public string FormatString { get; set; }
        public event Action OnValueChanged;

        private string _instanceName = null;

        public string GetFormattedString(string arg) => arg;
        
        public void Invoke() {
            OnValueChanged?.Invoke();
            _observers.Invoke(default);
        }
        
        public void Invoke(T value) {
            OnValueChanged?.Invoke();
            _observers.Invoke(value);
        }
        
        public bool IsEmpty() => _observers.IsEmpty();

        public object ToObject() => this;

        // IObservable<T>, IObserver<T>
        private DefaultObservers<T> _observers = new DefaultObservers<T>();
        public IDisposable Subscribe(IObserver<T> observer) {
            return _observers.Subscribe(observer, emitOnSubscribe:false, default);
        }

        public IDisposable SubscribeOnValueChanged(Action action) {
            OnValueChanged += action;
            return Disposable.Create(() => OnValueChanged -= action);
        }
    }
    
    public class CommandProperty : CommandProperty<Unit> { }
    public class CommandIntProperty : CommandProperty<int> { }
}