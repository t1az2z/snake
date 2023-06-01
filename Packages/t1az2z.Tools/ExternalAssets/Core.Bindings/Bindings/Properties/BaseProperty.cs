using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Core.Bindings.Tools.Rx;
using UnityEngine;

namespace Binding.Base {
    public class Property<T> : IProperty, IObservable<T>, IObserver<T> {
        public event Action OnValueChanged;

        string IProperty.InstanceName {
            get { return _instanceName; }
            set { _instanceName = value; }
        }

        public string FormatString { get; set; }

        public T Value {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        private T _value = default;
        private Type _type = null;
        private bool _changing = false;
        private string _instanceName = null;

        public Property(T startValue = default, string formatString = "") {
            _value = startValue;
            _type = typeof(T);
            FormatString = formatString;
        }

        public void SetValueAndMarkDirty(T newVal) {
            // if (_changing) {
            //     return;
            // }

            _changing = true;
            _value = newVal;
            ValueChanged();
            _changing = false;
        }

        protected virtual void SetValue(T newVal) {
            // if (_changing) {
            //     Dev.LogWarning($"Property {GetType()} is changing now. Ignoring!");
            //     return;
            // }

            _changing = true;

            var changed = !EqualityComparer<T>.Default.Equals(_value, newVal);
            if (changed) {
                _value = newVal;
                ValueChanged();
            }

            _changing = false;
        }

        protected virtual T GetValue() {
            return _value;
        }

        protected void ValueChanged() {
            OnValueChanged?.Invoke();
            _observers.Invoke(_value);
        }

        public bool IsEmpty() {
            if (Value is ITuple tuple) {
                for (int i = 0; i < tuple.Length; ++i) {
                    if (tuple[i] != null) {
                        return false;
                    }
                }
                return true;
            }

            if (Value is string str) {
                return string.IsNullOrEmpty(str);
            }

            return Value == null;
        }

        public object ToObject() {
            return Value;
        }

        public string GetFormattedString(string arg) {
            return string.IsNullOrEmpty(FormatString) ? arg : string.Format(FormatString, arg);
        }

        public override string ToString() {
            return string.IsNullOrEmpty(FormatString) ? Value?.ToString() : string.Format(FormatString, Value);
        }

        // IObservable<T>, IObserver<T>
        private DefaultObservers<T> _observers = new DefaultObservers<T>();
        public IDisposable Subscribe(IObserver<T> observer) {
            return _observers.Subscribe(observer, emitOnSubscribe: true, _value);
        }

        public IDisposable SubscribeOnValueChanged(Action action) {
            OnValueChanged += action;
            return Disposable.Create(() => OnValueChanged -= action);
        }

        public void OnNext(T value) {
            Value = value;
        }

        public void OnCompleted() { }

        public void OnError(Exception error) {
            Debug.LogError(error);
        }
    }
}