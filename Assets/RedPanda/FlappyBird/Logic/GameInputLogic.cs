using UnityEngine;
using WaterToolkit.Data;

namespace FlappyBird
{
	public class GameInputLogic : MonoBehaviour
	{
		private DefaultGameMode _gameMode = null;
		private DefaultTestUI _ui = null;

		private void InternalOnJumpButtonTap()
		{
			_gameMode.birdInstance.Jump();
		}

		private void Awake()
		{
			_gameMode = Blackboard.Get<DefaultGameMode>();
			_ui = Blackboard.Get<DefaultTestUI>();
		}

		private void Update()
		{
			if(_gameMode.gamePhase != DefaultGameMode.GamePhase.GameInit &&
			   _gameMode.gamePhase != DefaultGameMode.GamePhase.InGame) { 
				return; 
			}

			if(Input.GetMouseButtonDown(0)) { InternalOnJumpButtonTap(); }
		}

		//private void OnEnable()
		//{
		//	_ui.OnJumpButtonTap += InternalOnJumpButtonTap;
		//}

		//private void OnDisable()
		//{
		//	_ui.OnJumpButtonTap -= InternalOnJumpButtonTap;
		//}
	}
}
