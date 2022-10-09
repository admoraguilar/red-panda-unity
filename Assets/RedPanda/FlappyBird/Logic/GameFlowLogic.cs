using System;
using UnityEngine;
using WaterToolkit.Data;

namespace FlappyBird
{
	public class GameFlowLogic : MonoBehaviour
	{
		private DefaultGameMode _gameMode = null;
		private DefaultUI _ui = null;
		private FlowDirectoryData _flowDirectoryData = null;
		private Transition _transition = null;
		private Fader _fader = null;
		private Scorer _scorer = null;
		private DataSerializer dataSerializer = null;

		private void FadeTransition(Action onFinish)
		{
			bool isFinish = false;

			_transition.Enqueue(_fader.FadeTo(0, false), 0);
			_transition.Enqueue(
				_fader.FadeTo(
					1, true,
					(float fade) => {
						if(fade < .2f && !isFinish) {
							isFinish = true;
							onFinish();
						}
					})
				, 10);
			_transition.Run();
		}

		private void OnStartButtonTapMethod()
		{
			FadeTransition(() => _flowDirectoryData.inGame.Set());
		}

		private void OnRetryButtonTapMethod()
		{
			_gameMode.CleanupGame();
			FadeTransition(() => _flowDirectoryData.preGame.Set());
		}

		private void OnBirdCollideOnPipeMethod()
		{
			if(_gameMode.gamePhase == DefaultGameMode.GamePhase.GameOver) { return; }
			_flowDirectoryData.gameOver.Set();
		}

		private void OnBirdPassThruPipeMethod()
		{
			_scorer.current++;
			_ui.inGameScreen.SetCurrentScore(_scorer.current);
		}

		private void OnBirdCollideOnGroundMethod()
		{
			if(_gameMode.gamePhase == DefaultGameMode.GamePhase.GameOver) { return; }
			_flowDirectoryData.gameOver.Set();
		}

		private void OnFlowInitializeVisitMethod()
		{
			dataSerializer.Deserialize();
			ScoreSerializableData scoreData = dataSerializer.Get<ScoreSerializableData>();
			_scorer.ForceSet(0, scoreData.best);

			_flowDirectoryData.initialize.Next();
		}

		private void OnFlowPreGameVisitMethod()
		{
			_scorer.current = 0;
			_ui.inGameScreen.SetCurrentScore(_scorer.current);

			_ui.To(DefaultUI.Mode.PreGame);
		}

		private void OnFlowInGameVisitMethod()
		{
			_gameMode.StartGame();
			_ui.To(DefaultUI.Mode.InGame);
		}

		private void OnFlowGameOverVisitMethod()
		{
			_gameMode.StopGame();

			_ui.gameOverScreen.SetCurrentScoreText(_scorer.current);
			_ui.gameOverScreen.SetBestScoreText(_scorer.best);
			
			_ui.To(DefaultUI.Mode.GameOver);

			ScoreSerializableData scoreData = dataSerializer.Get<ScoreSerializableData>();
			scoreData.best = _scorer.best;
			dataSerializer.Serialize();
		}

		private void Awake()
		{
			_gameMode = Blackboard.Get<DefaultGameMode>();
			_ui = Blackboard.Get<DefaultUI>();
			_flowDirectoryData = Blackboard.Get<FlowDirectoryData>();
			_transition = Blackboard.Get<Transition>();
			_fader = Blackboard.Get<Fader>();
			_scorer = Blackboard.Get<Scorer>();
			dataSerializer = Blackboard.Get<DataSerializer>();
		}

		private void Start()
		{
			_flowDirectoryData.initialize.Set();
		}

		private void OnEnable()
		{
			_gameMode.OnBirdCollideOnPipe += OnBirdCollideOnPipeMethod;
			_gameMode.OnBirdPassThruPipe += OnBirdPassThruPipeMethod;
			_gameMode.OnBirdCollideOnGround += OnBirdCollideOnGroundMethod;
			_ui.preGameScreen.OnPlayButtonTap += OnStartButtonTapMethod;
			_ui.gameOverScreen.OnRetryButtonTap += OnRetryButtonTapMethod;
			_flowDirectoryData.initialize.OnVisit += OnFlowInitializeVisitMethod;
			_flowDirectoryData.preGame.OnVisit += OnFlowPreGameVisitMethod;
			_flowDirectoryData.inGame.OnVisit += OnFlowInGameVisitMethod;
			_flowDirectoryData.gameOver.OnVisit += OnFlowGameOverVisitMethod;
		}

		private void OnDisable()
		{
			_gameMode.OnBirdCollideOnPipe -= OnBirdCollideOnPipeMethod;
			_gameMode.OnBirdPassThruPipe -= OnBirdPassThruPipeMethod;
			_gameMode.OnBirdCollideOnGround -= OnBirdCollideOnGroundMethod;
			_ui.preGameScreen.OnPlayButtonTap -= OnStartButtonTapMethod;
			_ui.gameOverScreen.OnRetryButtonTap -= OnRetryButtonTapMethod;
			_flowDirectoryData.initialize.OnVisit -= OnFlowInitializeVisitMethod;
			_flowDirectoryData.preGame.OnVisit -= OnFlowPreGameVisitMethod;
			_flowDirectoryData.inGame.OnVisit -= OnFlowInGameVisitMethod;
			_flowDirectoryData.gameOver.OnVisit -= OnFlowGameOverVisitMethod;
		}
	}
}
