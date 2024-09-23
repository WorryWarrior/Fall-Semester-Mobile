using Content.Gameplay.Code.Camera;
using Content.Gameplay.Code.Progress;
using Content.Gameplay.Code.UI;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;

namespace Content.Gameplay.Code.Level
{
    public class LevelController : NetworkBehaviour
    {
        [Header("Player")]
        [SerializeField] private Transform playerPosition;
        [SerializeField] private GameObject playerPrefab;

        [Header("Camera")]
        [SerializeField] private CameraController cameraController;

        [Header("Progress")]
        [SerializeField] private ProgressController progressController;
        [SerializeField] private float progressDelta = 0.01f;

        [Header("Ambience")]
        [SerializeField] private Vector2Int areaDimensions = Vector2Int.one;
        [SerializeField] private float grassStep = 0.25f;
        [SerializeField] private GameObject[] grassPrefabs;

        [Header("UI")]
        [SerializeField] private RestartView restartView;

        private VolumeProfile _postProcessProfile;

        public bool IsServerInitialized { get; set; }
        private bool isInitialized = false;
        private bool isRunning = true;

        public void Initialize()
        {
            restartView.RestartButtonPressed = Restart;
            restartView.Initialize();

            CreateAmbience();
        }

        [Server]
        private void Update()
        {
            if (IsServerInitialized)
            {
                if (!isInitialized)
                {
                    CreateProgressModules();
                    isInitialized = true;
                }

                if (isRunning)
                {
                    progressController.Tick(progressDelta);
                }
            }
        }
        private void CreateAmbience()
        {
            Random.InitState(42);
            
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

        public GameObject CreatePlayer()
        {
            GameObject playerInstance = Instantiate(playerPrefab, playerPosition);

            //cameraController.SetFollowTarget(playerInstance.transform);

            return playerInstance;
        }

        private void Restart()
        {
            isRunning = true;
            progressController.Reset();
            restartView.Hide();
        }

        [Server]
        private void CreateProgressModules()
        {
            progressController.Reset();
            progressController.OnProgressEmptyReached += OnProgressEmpty;
            progressController.OnProgressLimitReached += OnProgressLimit;
        }

        [Server]
        private void OnProgressEmpty()
        {
            isRunning = false;
            restartView.Show(true, true);
        }

        [Server]
        private void OnProgressLimit()
        {
            isRunning = false;
            restartView.Show(false, true);
        }
    }
}