using UnityEngine;
using GameAnalyticsSDK;

namespace FlappyBird
{
	[RequireComponent(typeof(GameAnalytics))]
	public class GameAnalyticsWrapper : MonoBehaviour, IGameAnalyticsATTListener
	{
		[SerializeField]
		private GameAnalytics _sdkInstance = null;

		public GameAnalytics sdkInstance => _sdkInstance;

		public void Initialize()
		{
			if(Application.platform == RuntimePlatform.IPhonePlayer) {
				GameAnalytics.RequestTrackingAuthorization(this);
			} else {
				GameAnalytics.Initialize();
			}
		}

		public void GameStart()
		{
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "default_level");
		}

		public void GameFinish()
		{
			GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "default_level");
		}

		public void SubmitScore(int score)
		{
			GameAnalytics.NewDesignEvent("game:score", score);
		}

		void IGameAnalyticsATTListener.GameAnalyticsATTListenerNotDetermined() => GameAnalytics.Initialize();

		void IGameAnalyticsATTListener.GameAnalyticsATTListenerRestricted() => GameAnalytics.Initialize();

		void IGameAnalyticsATTListener.GameAnalyticsATTListenerDenied() => GameAnalytics.Initialize();

		void IGameAnalyticsATTListener.GameAnalyticsATTListenerAuthorized() => GameAnalytics.Initialize();
	}
}
