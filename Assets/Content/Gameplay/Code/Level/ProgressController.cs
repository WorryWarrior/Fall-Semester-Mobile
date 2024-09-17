using System;
using UnityEngine;

namespace Content.Gameplay.Code.Level
{
    public class ProgressController : MonoBehaviour
    {
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

        public event Action<float> OnProgressChanged;
        public event Action OnProgressLimitReached;

        public void Tick(float progressDelta, float progressLimit)
        {
            Progress = Mathf.Clamp(Progress + progressDelta, 0, progressLimit);

            if (Progress - progressLimit < 0.001f)
            {
                OnProgressLimitReached?.Invoke();
            }
        }
    }
}