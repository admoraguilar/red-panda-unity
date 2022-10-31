using UnityEngine;
using WaterToolkit.Blackboards;

namespace FlappyBird
{
	public class GameAnalyticsLogic : MonoBehaviour
	{
		[SerializeField]
		private bool _isEnableAnalytics = false;

		private GameAnalyticsWrapper _gameAnalyticsWrapper = null;
		private FlowDirectoryData _flowDirectoryData = null;
		
		private void OnFlowInitializeVisitMethod()
		{
			_gameAnalyticsWrapper.Initialize();
		}

		private void OnFlowInGameVisitMethod()
		{
			_gameAnalyticsWrapper.GameStart();
		}

		private void OnFlowGameOverVisitMethod()
		{
			_gameAnalyticsWrapper.GameFinish();
		}

		private void Awake()
		{
			_gameAnalyticsWrapper = SBlackboard.Get<GameAnalyticsWrapper>();
			_flowDirectoryData = SBlackboard.Get<FlowDirectoryData>();

			if(!_isEnableAnalytics || Application.isEditor) {
				Destroy(_gameAnalyticsWrapper.gameObject);
				Destroy(gameObject);
				return;
			}
		}

		private void OnEnable()
		{
			_flowDirectoryData.initialize.OnVisit += OnFlowInitializeVisitMethod;
			_flowDirectoryData.inGame.OnVisit += OnFlowInGameVisitMethod;
			_flowDirectoryData.gameOver.OnVisit += OnFlowGameOverVisitMethod;
		}

		private void OnDisable()
		{
			_flowDirectoryData.initialize.OnVisit -= OnFlowInitializeVisitMethod;
			_flowDirectoryData.inGame.OnVisit -= OnFlowInGameVisitMethod;
			_flowDirectoryData.gameOver.OnVisit -= OnFlowGameOverVisitMethod;
		}
	}
}
