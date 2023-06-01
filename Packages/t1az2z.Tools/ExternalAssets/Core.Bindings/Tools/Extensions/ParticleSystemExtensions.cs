using UnityEngine;

namespace Core.Bindings.Tools.Extensions {
    public static class ParticleSystemExtensions {
        /// <param name="isIndependentUpdate">If TRUE the particle system will ignore Unity's Time.timeScale</param>
        public static void SetUpdateMode(this ParticleSystem particleSystem, bool isIndependentUpdate) {
            var main = particleSystem.main;
            main.useUnscaledTime = isIndependentUpdate;
        }

        public static void SetUpdateMode(this ParticleSystem[] particleSystems, bool isIndependentUpdate) {
            foreach (var particleSystem in particleSystems) {
                particleSystem.SetUpdateMode(isIndependentUpdate);
            }
        }
    }
}