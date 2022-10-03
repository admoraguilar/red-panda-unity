using System;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace FlappyBird
{
	public class InputInterpreter
	{
		public event Action OnJump = delegate { };

		private MonoDelegate _mono = null;

		private MonoDelegate mono
		{
			get {
				_mono = UObject.Instantiate(_mono);
				_mono.OnUpdate += Update;
				UObject.DontDestroyOnLoad(_mono.gameObject);
				return _mono;
			}
		}

		private void PollJumpInput()
		{
			if(Input.GetKeyDown(KeyCode.F)) {
				OnJump();
			}
		}

		private void Update()
		{
			PollJumpInput();
		}

		private class MonoDelegate : MonoBehaviour
		{
			public event Action OnUpdate = delegate { };

			private void Update() => OnUpdate();
		}
	}
}
