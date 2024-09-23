using System;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Content.Gameplay.Code.Progress
{
    public class ProgressController : NetworkBehaviour
    {
        [SerializeField] private float progressLimit = 100f;
        [field: SerializeField] public float ProgressInitialValue { get; private set; } = 50f;
        [SerializeField] private Volume postProcessVolume;

        private float _progress;
        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnProgressChanged?.Invoke(_progress);
            }
        }

        private ColorAdjustments _colorAdjustments;
        private ColorAdjustments ColorAdjustments
        {
            get
            {
                if (_colorAdjustments == null)
                {
                    if (postProcessVolume.profile.TryGet(out ColorAdjustments ca))
                    {
                        _colorAdjustments = ca;
                    }
                }

                return _colorAdjustments;
            }
        }

        public event Action<float> OnProgressChanged;
        public event Action OnProgressLimitReached;
        public event Action OnProgressEmptyReached;

        public void Tick(float progressDelta)
        {
            Progress = Mathf.Clamp(Progress + progressDelta, 0, progressLimit);
            ColorAdjustments.saturation.value = -Progress;

            if (Progress >= progressLimit)
            {
                OnProgressLimitReached?.Invoke();
            }

            if (Progress <= 0f)
            {
                OnProgressEmptyReached?.Invoke();
            }
        }

        public void Reset()
        {
            Progress = ProgressInitialValue;
        }
    }
}