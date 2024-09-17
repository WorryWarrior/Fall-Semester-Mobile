using Content.Gameplay.Code.Analytics;
using Content.Gameplay.Code.Camera;
using Content.Gameplay.Code.Movement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Content.Gameplay.Code.Level
{
    public class LevelController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Transform playerPosition;
        [SerializeField] private GameObject playerPrefab;

        [Header("Camera")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private Volume postProcessVolume;

        [Header("Progress")]
        [SerializeField] private ProgressController progressController;
        [SerializeField] private float progressInitialValue = 50f;
        [SerializeField] private float progressDelta = 0.01f;
        [SerializeField] private float progressLimit = 100f;

        [Header("Input")]
        [SerializeField] private Joystick joystick;

        [Header("Ambience")]
        [SerializeField] private Vector2Int areaDimensions = Vector2Int.one;
        [SerializeField] private float grassStep = 0.25f;
        [SerializeField] private GameObject[] grassPrefabs;

        private CharacterMovementController _characterMovementController;
        private VolumeProfile _postProcessProfile;

        public void Initialize()
        {
            CreateProgressModules();
            CreateAmbience();
            CreatePlayer();

        }

        private void Update()
        {
            _characterMovementController.Tick(joystick.Direction);
            progressController.Tick(progressDelta, progressLimit);
        }

        private void CreateAmbience()
        {
            GameObject grassParent = new GameObject("Ambience_Grass");
            grassParent.transform.SetParent(transform);

            for (int x = -areaDimensions.x / 2; x < areaDimensions.x / 2; x++)
            {
                for (int y = -areaDimensions.y / 2; y < areaDimensions.y / 2; y++)
                {
                    Vector3 grassBladeLocalOffset = new Vector3(Random.Range(-grassStep * 0.5f, grassStep * 0.5f), 0,
                        Random.Range(-grassStep * 0.5f, grassStep * 0.5f));

                    GameObject grassInstance = Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Length)],
                        new Vector3(x, 0, y) * grassStep + grassBladeLocalOffset, Quaternion.identity);

                    grassInstance.transform.SetParent(grassParent.transform);
                }
            }
        }

        private void CreatePlayer()
        {
            GameObject playerInstance = Instantiate(playerPrefab, playerPosition);

            cameraController.SetFollowTarget(playerInstance.transform);

            _characterMovementController = playerInstance.GetComponent<CharacterMovementController>();
        }

        private void CreateProgressModules()
        {
            _postProcessProfile = postProcessVolume.profile;
            progressController.OnProgressChanged += ProcessProgressChange;
            progressController.Progress = progressInitialValue;
        }

        private void ProcessProgressChange(float value)
        {
            if (_postProcessProfile.TryGet(out ColorAdjustments colorAdjustments))
            {
                colorAdjustments.saturation.SetValue(new FloatParameter(-value));
            }
        }
    }
}