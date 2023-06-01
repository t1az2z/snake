using System;
using Core.Bindings.Tools.Extensions;
using UnityEngine.LowLevel;

namespace Core.Bindings.Tools.Helpers {
    public static class PlayerLoopUtils {
        public static bool SetBefore<T>(ref PlayerLoopSystem system, PlayerLoopSystem.UpdateFunction function, Type type) {
            if (system.subSystemList == null) {
                return false;
            }

            for (int i = 0; i < system.subSystemList.Length; i++) {
                if (system.subSystemList[i].type == typeof(T)) {
                    var newSystem = new PlayerLoopSystem();
                    newSystem.updateDelegate += function;
                    newSystem.type = type;
                    system.subSystemList = system.subSystemList.Insert(i, newSystem);
                    return true;
                }

                if (SetBefore<T>(ref system.subSystemList[i], function, type)) {
                    return true;
                }
            }

            return false;
        }

        public static bool SetAfter<T>(ref PlayerLoopSystem system, PlayerLoopSystem.UpdateFunction function, Type type) {
            if (system.subSystemList == null) {
                return false;
            }

            for (int i = 0; i < system.subSystemList.Length; i++) {
                if (system.subSystemList[i].type == typeof(T)) {
                    var newSystem = new PlayerLoopSystem();
                    newSystem.updateDelegate += function;
                    newSystem.type = type;
                    system.subSystemList = system.subSystemList.Insert(i + 1, newSystem);
                    return true;
                }

                if (SetAfter<T>(ref system.subSystemList[i], function, type)) {
                    return true;
                }
            }

            return false;
        }
    }
}