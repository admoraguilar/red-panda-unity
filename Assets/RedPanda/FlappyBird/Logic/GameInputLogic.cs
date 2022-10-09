using UnityEngine;
using WaterToolkit.Data;

namespace FlappyBird
{
	public class GameInputLogic : MonoBehaviour
	{
		private DefaultGameMode _gameMode = null;

		private void InternalOnJumpButtonTap()
		{
			_gameMode.birdInstance.Jump();
		}

		private void Awake()
		{
			_gameMode = Blackboard.Get<DefaultGameMode>();
		}

		private void Update()
		{
			if(!_gameMode.isInGame) { return; }

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
