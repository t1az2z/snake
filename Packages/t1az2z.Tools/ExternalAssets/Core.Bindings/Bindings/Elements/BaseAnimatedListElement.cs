using System.Collections;
using System.Threading.Tasks;
using Core.Bindings.Tools.Extensions;
using UnityEngine;

namespace Binding.Elements {
    [RequireComponent(typeof(Animator))]
    public abstract class BaseAnimatedListElement : BaseListElement {
        private static readonly int _showHash = Animator.StringToHash("Show");
        private static readonly int _hideHash = Animator.StringToHash("Hide");
        private static readonly int _showImmediateHash = Animator.StringToHash("ShowImmediate");
        private static readonly int _hideImmediateHash = Animator.StringToHash("HideImmediate");

        public bool ShowInProgress { get; private set; }
        public bool HideInProgress { get; private set; }
        
        [SerializeField] private float ShowDuration = 0.5f;
        [SerializeField] private float HideDuration = 0.5f;

        private Animator _animator;
        private Coroutine _animationRoutine;

        private void Awake() {
            _animator = GetComponent<Animator>();

#if UNITY_EDITOR
            if (_animator == null) {
                Debug.LogErrorFormat("Animated list element \"{0}\" doesn't contains animator!", gameObject.name);
            }
#endif
        }

        private void OnDestroy() {
            ResetAnimation();
        }

        public async Task ShowAsync(bool instant) {
            if (_animator == null || !_animator.HasParameter(_showHash) || !_animator.HasParameter(_showImmediateHash) || ShowInProgress) {
                return;
            }

            StartCoroutine(ShowCoroutine(instant));
            while (ShowInProgress) {
                await Task.Yield();
            }
        }

        public async Task HideAsync(bool instant) {
            if (_animator == null || !_animator.HasParameter(_hideHash) || !_animator.HasParameter(_hideImmediateHash) || HideInProgress) {
                return;
            }

            StartCoroutine(HideCoroutine(instant));
            while (HideInProgress) {
                await Task.Yield();
            }
        }

        public void ResetAnimation() {
            if (_animator == null) {
                return;
            }

            ShowInProgress = false;
            HideInProgress = false;

            StopAllCoroutines();
        }

        private IEnumerator ShowCoroutine(bool instant) {
            ShowInProgress = true;

            while (HideInProgress) {
                yield return null;
            }

            _animator.SetTrigger(instant ? _showImmediateHash : _showHash);
            _animator.Update(0f);

            yield return new WaitForSecondsRealtime(ShowDuration);

            ShowInProgress = false;
        }

        private IEnumerator HideCoroutine(bool instant) {
            HideInProgress = true;

            while (ShowInProgress) {
                yield return null;
            }

            _animator.SetTrigger(instant ? _hideImmediateHash : _hideHash);
            _animator.Update(0f);

            yield return new WaitForSecondsRealtime(HideDuration);

            HideInProgress = false;
        }
    }
}