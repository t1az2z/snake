using System.Text;
using Binding.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Bindings.Components
{
    public class BaseBinding : MonoBehaviour
    {
        [FormerlySerializedAs("_path")] [SerializeField, BindingProperty]
        private string Path = "";

        public string PropertyName => Path;

        public IProperty Property { get; private set; }
        protected IBindingTarget Target { get; private set; }
        protected virtual bool EmptyPathAllowed => false;

        protected virtual void Awake()
        {
            if (FindTarget())
            {
                Bind(true);
                if (Property != null && !(Property is ICommand))
                    OnPropertyValueUpdated();
            }
        }

        protected virtual void OnDestroy()
        {
            Unbind(true);
        }

        protected virtual void OnPropertyValueUpdated()
        {
            if (Property != null)
            {
                OnValueUpdated();
            }
        }

        protected virtual void OnForceUpdate()
        {
            if (Property != null)
            {
                Unbind();
                UpdateProperty();
                Bind();
                OnPropertyValueUpdated();
            }
        }

        protected virtual void OnValueUpdated()
        {
        }

        protected virtual bool IsPropertyValid(IProperty property)
        {
            return property != null;
        }

        protected virtual void Bind(bool total = false)
        {
            if (Property != null)
            {
                Property.OnValueChanged += OnPropertyValueUpdated;

                if (total)
                {
                    Target.OnForceUpdateProperties += OnForceUpdate;
                }
            }
        }

        protected virtual void Unbind(bool total = false)
        {
            if (Property != null)
            {
                Property.OnValueChanged -= OnPropertyValueUpdated;

                if (total)
                {
                    Target.OnForceUpdateProperties -= OnForceUpdate;
                }
            }
        }

        protected void UpdateProperty()
        {
            if (Target != null)
            {
                Property = Target.GetProperty(Path);
            }
        }

        private bool _internalCallOfFindProperty = false;

        protected (IBindingTarget, T) FindProperty<T>(string path) where T : class, IProperty
        {
            var emptyPath = string.IsNullOrEmpty(path);
            if (EmptyPathAllowed && emptyPath)
            {
                return (null, default);
            }

            if (!emptyPath)
            {
                var parent = transform;
                while (parent != null)
                {
                    var targets = parent.GetComponents<IBindingTarget>();
                    foreach (var target in targets)
                    {
                        var property = target.GetProperty(path);
                        var good = _internalCallOfFindProperty
                            ? IsPropertyValid(property)
                            : (property != null && property is T);
                        if (good)
                        {
                            return (target, property as T);
                        }
                    }

                    parent = parent.parent;
                }
            }

            Debug.LogErrorFormat("[Binding] BindingTarget \"{0}\" by path \"{1}\" \nTransform: \"{2}\"", GetType().Name,
                Path, GetFullPath());
            return (null, default);
        }

        private bool FindTarget()
        {
            _internalCallOfFindProperty = true;
            try
            {
                (this.Target, this.Property) = FindProperty<IProperty>(Path);
            }
            finally
            {
                _internalCallOfFindProperty = false;
            }

            return this.Property != null;
        }

        private string GetFullPath()
        {
            var builder = new StringBuilder();
            builder.Append(transform.name);

            var parent = transform.parent;
            while (parent != null)
            {
                builder.Insert(0, $"{parent.name}/");
                parent = parent.parent;
            }

            return builder.ToString();
        }
    }

    public class BaseBinding<T> : BaseBinding where T : class, IProperty
    {
        protected new T Property
        {
            get { return base.Property as T; }
        }

        protected override bool IsPropertyValid(IProperty property)
        {
            return (property != null) && (property is T);
        }
    }
}