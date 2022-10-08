using UnityEngine;
using WaterToolkit.Data;

namespace FlappyBird
{
	public class GameInputLogic : MonoBehaviour
	{
		private DefaultGameMode _gameMode = null;
		private DefaultUI _ui = null;

		private void InternalOnJumpButtonTap()
		{
			_gameMode.birdInstance.Jump();
		}

		private void Awake()
		{
			_gameMode = Blackboard.Get<DefaultGameMode>();
			_ui = Blackboard.Get<DefaultUI>();
		}

		private void OnEnable()
		{
			_ui.OnJumpButtonTap += InternalOnJumpButtonTap;
		}

		private void OnDisable()
		{
			_ui.OnJumpButtonTap -= InternalOnJumpButtonTap;
		}
	}
}
