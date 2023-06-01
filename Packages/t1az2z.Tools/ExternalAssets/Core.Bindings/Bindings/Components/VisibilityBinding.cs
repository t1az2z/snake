using System;
using UnityEngine;

namespace Core.Bindings.Components {
    public class VisibilityBinding : BoolBinding {
        private readonly int ShowHash = Animator.StringToHash("Show");
        private readonly int HideHash = Animator.StringToHash("Hide");

        [SerializeField] private GameObject OverrideTarget;
        [SerializeField] private VisibilityBindingModeEnum Mode;
        
        [SerializeField] private float AnimatorScale = 1f;

        [Obsolete("Use 'Mode' instead")]
        [HideInInspector]
        [SerializeField] private bool CaptureAnimator = false;

        private bool _animatorInitiated = false;
        private Animator _targetAnimator;
        private GameObject _target;

        protected override void Awake() {
            Migrate();
            _target = OverrideTarget == null ? gameObject : OverrideTarget;
            
            switch (Mode) {
                case VisibilityBindingModeEnum.GameObject:
                    break;
                case VisibilityBindingModeEnum.Animator:
                    _targetAnimator = _target.GetComponent<Animator>();
                    if (_targetAnimator != null) {
                        _targetAnimator.keepAnimatorControllerStateOnDisable = true;
                        _targetAnimator.speed = 1f / Mathf.Max(0.01f, AnimatorScale);
                    }
                    break;
            }
            base.Awake();
        }

        private void OnValidate() => Migrate();

        private void Migrate() {
            if (CaptureAnimator) {
                CaptureAnimator = false;
                Mode = VisibilityBindingModeEnum.Animator;
            }
        }

        protected override void OnValueUpdated() {
            if (_targetAnimator) {
                if (BoolValue) {
                    _targetAnimator.ResetTrigger(HideHash);
                    _targetAnimator.SetTrigger(ShowHash);
                }
                else {
                    _targetAnimator.ResetTrigger(ShowHash);
                    _targetAnimator.SetTrigger(HideHash);
                }

                if (!_animatorInitiated) {
                    _animatorInitiated = true;
                    _targetAnimator.Update(0f);  // apply triggers
                    _targetAnimator.Update(999f);// go to the end of animation
                }
            }
            else {
                _target.SetActive(BoolValue);
            }
        }

        private enum VisibilityBindingModeEnum {
            GameObject = 0,
            Animator = 1
        }
    }
}