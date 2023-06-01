using System;
using System.Collections.Generic;
using System.Reflection;

namespace Binding.Base {
    public interface IBindingTarget {
        event Action OnForceUpdateProperties;

        MethodInfo GetMethod(string name);
        IProperty GetProperty(string name);
        bool Editor_HasProperty(string name);
        Dictionary<string, FieldInfo> Editor_GetPropertiesFlatmap();
    }

    public class NotBindingTargetAttribute : Attribute {};
}