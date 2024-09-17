using System.Threading.Tasks;
using Content.Gameplay.Code.Analytics;
using Content.Gameplay.Code.Level;
using Firebase.Crashlytics;
using UnityEngine;

namespace Content.Gameplay.Code.GameLoop
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private GameObject levelPrefab;
        [SerializeField] private AnalyticsController analyticsController;

        private async void Awake()
        {
            await InitializeCrashlytics();
            analyticsController.LogEvent(EventNames.Session);

            LevelController levelController = Instantiate(levelPrefab).GetComponent<LevelController>();
            levelController.Initialize();
        }

        private Task InitializeCrashlytics()
        {
            return Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                    Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                }
                else
                {
                    Debug.LogError(string.Format("Could not resolve all Firebase dependencies: {0}",dependencyStatus));
                }
            });
        }
    }
}