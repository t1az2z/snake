using System;
using System.Linq;
using Binding;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Core.Bindings.Components {
    public class ButtonIsPressedBinding : BaseBinding<BoolProperty> {
        private EventTrigger _trigger;

        protected override void Awake() {
            _trigger = GetComponent<EventTrigger>();
            base.Awake();
        }

        protected override void Bind(bool total = false) {
            base.Bind(total);
            
            if (_trigger == null) 
                return;

            var downTrigger = GetOrCreateTrigger(EventTriggerType.PointerDown);
            downTrigger.callback.AddListener(NotifyPressed);

            var upTrigger = GetOrCreateTrigger(EventTriggerType.PointerUp);
            upTrigger.callback.AddListener(NotifyUnpressed);
        }

        protected override void Unbind(bool total = false) {
            base.Unbind(total);
            
            if (_trigger == null)
                return;
            
            var downTrigger = GetOrCreateTrigger(EventTriggerType.PointerDown);
            downTrigger.callback.RemoveListener(NotifyPressed);

            var upTrigger = GetOrCreateTrigger(EventTriggerType.PointerUp);
            upTrigger.callback.RemoveListener(NotifyUnpressed);
        }

        private EventTrigger.Entry GetOrCreateTrigger(EventTriggerType type) {
            var trigger = _trigger.triggers.FirstOrDefault(t => t.eventID == type);
            if (trigger == null) {
                trigger = new EventTrigger.Entry {
                    eventID = type
                };
                _trigger.triggers.Add(trigger);
            }

            return trigger;
        }

        private void OnDisable() {
            Property?.OnNext(false);
        }

        private void NotifyPressed(BaseEventData _) => Property?.OnNext(true);
        
        private void NotifyUnpressed(BaseEventData _) => Property?.OnNext(false);
    }
}