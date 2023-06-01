using Binding.Base;
using System;
using UnityEngine;

namespace Binding {
    public class BaseListElementData : BaseBindingTarget, IComparable<BaseListElementData> {
        public event Action<BaseListElementData> OnClick;
        public event Action<BaseListElementData> OnBecameVisible;
        public event Action<BaseListElementData> OnBecameInvisible;
        public event Action<BaseListElementData> OnEnterScope;
        public event Action<BaseListElementData> OnLeaveScope;
        public event Action<BaseListElementData> OnRemove;

        public OffScreenModes OffScreenMode = OffScreenModes.OnScreen;
        public Vector2 Size;

        public virtual int Sort { get; set; }

        public bool IsInScope => _inScope.Value;
        public bool IntersectViewportCenter => _intersectViewportCenter.Value;
        
        private BoolProperty _visible = new BoolProperty();
        private BoolProperty _inScope = new BoolProperty();
        private BoolProperty _intersectViewportCenter = new BoolProperty();

        public virtual void OnRemoved() {
            OnRemove?.Invoke(this);
            OnRemove = null;
        }

        public void ClickFromUI() {
            OnClick?.Invoke(this);
        }

        // IComparable
        public int CompareTo(BaseListElementData other) {
            return Sort.CompareTo(other.Sort);
        }

        public void UpdateOffScreen(float minViewportPosition, float maxViewportPosition, float minPosition, float maxPosition, float scopeOffset = 0) {
            var halfSize = 0.5f * (maxPosition - minPosition);
            OffScreenMode = OffScreenModes.OnScreen;
            if (maxPosition > maxViewportPosition) {
                OffScreenMode |= OffScreenModes.HiddenByMaxBorder;
                if (minPosition < maxViewportPosition) {
                    OffScreenMode |= OffScreenModes.IntersectBorder;
                }

                if (minPosition + halfSize > maxViewportPosition) {
                    OffScreenMode |= OffScreenModes.HiddenCenterByMaxBorder;
                }

                if (minPosition - scopeOffset <= maxViewportPosition) {
                    OffScreenMode |= OffScreenModes.AlmostOnScreen;
                }
            }

            if (minPosition < minViewportPosition) {
                OffScreenMode |= OffScreenModes.HiddenByMinBorder;
                if (maxPosition > minViewportPosition) {
                    OffScreenMode |= OffScreenModes.IntersectBorder;
                }

                if (maxPosition - halfSize < minViewportPosition) {
                    OffScreenMode |= OffScreenModes.HiddenCenterByMinBorder;
                }

                if (maxPosition + scopeOffset >= minViewportPosition) {
                    OffScreenMode |= OffScreenModes.AlmostOnScreen;
                }
            }

            var isVisible = OffScreenMode == OffScreenModes.OnScreen || ((OffScreenMode & OffScreenModes.IntersectBorder) == OffScreenModes.IntersectBorder);
            var isInScope = OffScreenMode == OffScreenModes.OnScreen || ((OffScreenMode & OffScreenModes.AlmostOnScreen) == OffScreenModes.AlmostOnScreen);

            if (_visible.Value != isVisible) {
                _visible.Value = isVisible;
                if (isVisible) OnBecameVisible?.Invoke(this);
                else OnBecameInvisible?.Invoke(this);
            }

            if (_inScope.Value != isInScope) {
                _inScope.Value = isInScope;
                if (isInScope) OnEnterScope?.Invoke(this);
                else OnLeaveScope?.Invoke(this);
            }
            
            var viewportCenter = minViewportPosition + (maxViewportPosition - minViewportPosition) * 0.5f;
            _intersectViewportCenter.Value = minPosition < viewportCenter && maxPosition > viewportCenter;
        }

        [Flags]
        public enum OffScreenModes {
            OnScreen = 0,
            HiddenByMinBorder = 1 << 0,
            IntersectBorder = 1 << 1,
            HiddenByMaxBorder = 1 << 2,
            HiddenCenterByMaxBorder = 1 << 3,
            HiddenCenterByMinBorder = 1 << 4,
            AlmostOnScreen = 1 << 5
        }
    }
}