using System.Collections;
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

		private void OnStartButtonTapMethod()
		{
			bool isSetFlow = false;

			_transition.Enqueue(_fader.FadeTo(0, false), 0);
			_transition.Enqueue(
				_fader.FadeTo(
					1, true, 
					(float fade) => {
						if(fade < .2f && !isSetFlow) {
							isSetFlow = true;
							_flowDirectoryData.game.Set(); 
						} 
					})
				, 10);
			_transition.Run();
		}

		private void OnRetryButtonTapMethod()
		{
			bool isSetFlow = false;

			_transition.Enqueue(_fader.FadeTo(0, false), 0);
			_transition.Enqueue(
				_fader.FadeTo(
					1, true,
					(float fade) => {
						if(fade < .2f && !isSetFlow) {
							isSetFlow = true;
							_flowDirectoryData.menu.Set(); 
						}
					})
				, 10);
			_transition.Run();
		}

		private void OnBirdCollideOnPipeMethod()
		{
			_flowDirectoryData.gameOver.Set();
		}

		private void OnFlowMenuVisitMethod()
		{
			

			_ui.To(DefaultUI.Mode.StartMenu);
		}

		private void OnFlowGameVisitMethod()
		{
			_gameMode.StartGame();
			_ui.To(DefaultUI.Mode.InGame);
		}

		private void OnFlowGameOverVisitMethod()
		{
			_gameMode.StopGame();
			_ui.To(DefaultUI.Mode.GameOver);
		}

		private void Awake()
		{
			_gameMode = Blackboard.Get<DefaultGameMode>();
			_ui = Blackboard.Get<DefaultUI>();
			_flowDirectoryData = Blackboard.Get<FlowDirectoryData>();
			_transition = Blackboard.Get<Transition>();
			_fader = Blackboard.Get<Fader>();
		}

		private void Start()
		{
			_flowDirectoryData.menu.Set();
		}

		private void OnEnable()
		{
			_gameMode.OnBirdCollideOnPipe += OnBirdCollideOnPipeMethod;
			_ui.OnStartButtonTap += OnStartButtonTapMethod;
			_ui.OnRetryButtonTap += OnRetryButtonTapMethod;
			_flowDirectoryData.menu.OnVisit += OnFlowMenuVisitMethod;
			_flowDirectoryData.game.OnVisit += OnFlowGameVisitMethod;
			_flowDirectoryData.gameOver.OnVisit += OnFlowGameOverVisitMethod;
		}

		private void OnDisable()
		{
			_gameMode.OnBirdCollideOnPipe -= OnBirdCollideOnPipeMethod;
			_ui.OnStartButtonTap -= OnStartButtonTapMethod;
			_ui.OnRetryButtonTap -= OnRetryButtonTapMethod;
			_flowDirectoryData.menu.OnVisit -= OnFlowMenuVisitMethod;
			_flowDirectoryData.game.OnVisit -= OnFlowGameVisitMethod;
			_flowDirectoryData.gameOver.OnVisit -= OnFlowGameOverVisitMethod;
		}
	}
}
