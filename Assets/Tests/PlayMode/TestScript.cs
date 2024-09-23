using System.Collections;
using Content.Gameplay.Code.Progress;
using Mirror;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class TestScript
    {
        private GameObject _testGameObject;
        private ProgressController _progressController;

        [SetUp]
        public void SetUp()
        {
            _testGameObject = new GameObject("TestObject");
            _testGameObject.AddComponent<NetworkIdentity>();
            _progressController = _testGameObject.AddComponent<ProgressController>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_testGameObject);
        }

        [UnityTest]
        public IEnumerator ProgressController_ProgressEqualsInitialValueAfterReset()
        {
            _progressController.Reset();

            yield return null;

            Assert.AreEqual(_progressController.ProgressInitialValue, _progressController.Progress,
                "Progress should be equal to progressInitialValue after Reset.");
        }
    }
}