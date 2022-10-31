using System;
using UnityEngine;
using WaterToolkit.Scores;
using WaterToolkit.Sequences;
using WaterToolkit.Blackboards;

namespace FlappyBird
{
	public class GameFlowLogic : MonoBehaviour
	{
		private DefaultGameMode _gameMode = null;
		private DefaultUI _ui = null;
		private FlowDirectoryData _flowDirectoryData = null;
		private Fader _fader = null;

		private void FadeTransition(Action onFinish)
		{
			bool isFinish = false;

			Sequencer sequencer = new Sequencer();
			sequencer.Enqueue(_fader.FadeTo(0, false), 0);
			sequencer.Enqueue(
				_fader.FadeTo(
					1, true,
					(float fade) => {
						if(fade < .2f && !isFinish) {
							isFinish = true;
							onFinish();
						}
					})
				, 10);
			sequencer.Run();
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
			if(!_gameMode.isInGame) { return; }
			_flowDirectoryData.gameOver.Set();
		}

		private void OnBirdPassThruPipeMethod()
		{
			SScorer.current++;
			_ui.inGameScreen.SetCurrentScore(SScorer.current);
		}

		private void OnBirdCollideOnGroundMethod()
		{
			if(!_gameMode.isInGame) { return; }
			_flowDirectoryData.gameOver.Set();
		}

		private void OnFlowInitializeVisitMethod()
		{
			SFlappyGameSerialzer.gameData.Deserialize();
			ScoreSerializableData scoreData = SFlappyGameSerialzer.gameData.Get<ScoreSerializableData>();
			SScorer.ForceSet(0, scoreData.best);

			_flowDirectoryData.initialize.Next();
		}

		private void OnFlowPreGameVisitMethod()
		{
			_gameMode.PreGame();
			SScorer.current = 0;
			_ui.inGameScreen.SetCurrentScore(SScorer.current);

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

			_ui.gameOverScreen.SetCurrentScoreText(SScorer.current);
			_ui.gameOverScreen.SetBestScoreText(SScorer.best);
			
			_ui.To(DefaultUI.Mode.GameOver);

			ScoreSerializableData scoreData = SFlappyGameSerialzer.gameData.Get<ScoreSerializableData>();
			scoreData.best = SScorer.best;
			SFlappyGameSerialzer.gameData.Serialize();
		}

		private void Awake()
		{
			SBlackboard.AddResolvable(
				this,
				() => {
					return _gameMode == null || _ui == null ||
						   _flowDirectoryData == null || _fader == null;
				},
				() => {
					_gameMode = SBlackboard.Get<DefaultGameMode>();
					_ui = SBlackboard.Get<DefaultUI>();
					_flowDirectoryData = SBlackboard.Get<FlowDirectoryData>();
					_fader = SBlackboard.Get<Fader>();
				});
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
