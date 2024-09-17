using Firebase.Analytics;
using UnityEngine;

namespace Content.Gameplay.Code.Analytics
{
    public class AnalyticsController : MonoBehaviour
    {
        public void LogEvent(string eventName, params Parameter[] eventParameters)
        {
            FirebaseAnalytics.LogEvent(eventName, eventParameters);
        }
    }
}