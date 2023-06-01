using System.Reflection;
using System.Text;
using Binding.Base;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Bindings.Components {
    // TODO: refactor to CommandProperty?
    public class ClickBinding : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
        [SerializeField, BindingMethod(Type = BindingMethodType.Action)] private string Path = "";

        protected MethodInfo Method { get; private set; }
        protected IBindingTarget Target { get; private set; }
        
        protected virtual void Awake() {
            FindTarget();
        }

        private void Invoke() {
            if (Method != null) {
                Method.Invoke(Target, null);
            }
        }

        private bool FindTarget() {
            var emptyPath = string.IsNullOrEmpty(Path);
            // if (emptyPath) {
            //     return false;
            // }

            if (!emptyPath) {
                var parent = transform;
                while (parent != null) {
                    var target = parent.GetComponent<IBindingTarget>();
                    if (target != null) {                        
                        var method = target.GetMethod(Path);
                        if (IsMethodValid(method)) {
                            this.Method = method;
                            this.Target = target;
                            return true;
                        }
                    }

                    parent = parent.parent;
                }
            }
#if UNITY_EDITOR
            Debug.LogErrorFormat("[Binding] BindingTarget wasn't found for \"{0}\" by path \"{1}\" \nTransform: \"{2}\"", GetType().Name, Path,
                GetFullPath());
#endif
            return false;
        }

        private string GetFullPath() {
            var builder = new StringBuilder();
            builder.Append(transform.name);

            var parent = transform.parent;
            while (parent != null) {
                builder.Insert(0, $"{parent.name}/");
                parent = parent.parent;
            }

            return builder.ToString();
        }

        protected bool IsMethodValid(MethodInfo method) {
            return (method != null);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
            Invoke();
        }
    }
}