using DG.Tweening;
using UnityEngine;

namespace t1az2z.Tools.Tools.Common
{
    public class UiScaleLoop : MonoBehaviour
    {
        [SerializeField] private float TargetScale;
        [SerializeField] private float Time;
        Sequence _seq;

        private void OnEnable()
        {
            _seq?.Play();
        }

        private void OnDisable()
        {
            _seq?.Pause();
        }

        private void Awake()
        {
            _seq = DOTween.Sequence();
            var startScale = transform.localScale;
            _seq.Append(transform.DOScale(TargetScale, Time)).Append(transform.DOScale(startScale, Time)).SetLoops(-1);
        }
    }
}