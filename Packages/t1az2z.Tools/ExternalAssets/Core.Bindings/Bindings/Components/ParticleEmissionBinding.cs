using Binding;
using Binding.Base;
using UnityEngine;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleEmissionBinding : BaseBinding<IProperty> {
        private ParticleSystem _particles;

        protected override void Awake() {
            _particles = GetComponent<ParticleSystem>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            switch (Property) {
                case BoolProperty b:
                    if (b.Value) _particles.Play();
                    else _particles.Stop();
                    break;
                case CommandProperty _:
                    _particles.Play();
                    break;
            }
        }
    }
}