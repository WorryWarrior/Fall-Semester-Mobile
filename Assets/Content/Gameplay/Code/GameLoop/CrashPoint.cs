using System;
using UnityEngine;

namespace Content.Gameplay.Code.GameLoop
{
    public class CrashPoint : MonoBehaviour
    {
        private int _updatesBeforeException;

        private void Start()
        {
            _updatesBeforeException = 0;
        }

        private void Update()
        {
            ThrowExceptionEvery60Updates();
        }

        private void ThrowExceptionEvery60Updates()
        {
            if (_updatesBeforeException > 0)
            {
                _updatesBeforeException--;
            }
            else
            {
                _updatesBeforeException = 60;

                throw new Exception("Test exception please ignore");
            }
        }
    }
}