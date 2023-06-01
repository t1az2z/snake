using Binding;
using Binding.Base;
using Core.Bindings.Tools.Extensions;
using UnityEngine;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(Animator))]
    public class AnimatorParameterBinding : BaseBinding<IProperty> {
        [SerializeField] private string Parameter;
        
        private Animator _animator;
        private int _parameterHash;
        
        protected override void Awake() {
            _animator = GetComponent<Animator>();
            _parameterHash = string.IsNullOrEmpty(Parameter) || !_animator.HasParameter(Parameter) 
                                 ? -1 : Animator.StringToHash(Parameter);
            base.Awake();
        }

        protected override void OnValueUpdated() {
            if (_parameterHash == -1) return;
            
            switch (Property) {
                case BoolProperty boolProperty:
                    _animator.SetBool(_parameterHash, boolProperty.Value);
                    break;
                case IntProperty intProperty:
                    _animator.SetInteger(_parameterHash, intProperty.Value);
                    break;
                case FloatProperty floatProperty:
                    _animator.SetFloat(_parameterHash, floatProperty.Value);
                    break;
                case CommandProperty _: 
                    _animator.SetTrigger(_parameterHash);
                    break;
                default:
                    Debug.LogError($"{nameof(AnimatorParameterBinding)}> Unsupported property of type {Property.GetType()}");
                    break;
            }
        }
    }
}