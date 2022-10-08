using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBird
{
	public class DefaultUI : MonoBehaviour
	{
		public enum Mode
		{
			StartMenu,
			InGame,
			GameOver
		}

		public event Action OnStartButtonTap = delegate { };
		public event Action OnRetryButtonTap = delegate { };
		public event Action OnJumpButtonTap = delegate { };

		[SerializeField]
		private TMP_Text _flowText = null;

		[SerializeField]
		private Button _menuButton = null;

		[SerializeField]
		private Button _jumpButton = null;

		private Mode _mode = default;

		public Mode mode
		{
			get => _mode;
			private set => _mode = value;
		}

		public void To(Mode mode)
		{
			this.mode = mode;

			if(this.mode == Mode.StartMenu) { 
				_flowText.text = "Play";
				_menuButton.GetComponentInChildren<TMP_Text>().text = "Play";
			} else if(this.mode == Mode.InGame) { 
				_flowText.text = "In-Game"; 
			} else if(this.mode == Mode.GameOver) { 
				_flowText.text = "Retry";
				_menuButton.GetComponentInChildren<TMP_Text>().text = "Retry";
			}

			if(this.mode == Mode.InGame) {
				_menuButton.gameObject.SetActive(false);
				_jumpButton.gameObject.SetActive(true);
			} else {
				_menuButton.gameObject.SetActive(true);
				_jumpButton.gameObject.SetActive(false);
			}
		}

		private void OnMenuButtonTapMethod()
		{
			if(mode == Mode.StartMenu) { OnStartButtonTap(); }
			else if(mode == Mode.GameOver) { OnRetryButtonTap(); }
		}

		private void OnJumpButtonTapMethod()
		{
			if(mode == Mode.InGame) { OnJumpButtonTap(); }
		}

		private void OnEnable()
		{
			_menuButton.onClick.AddListener(OnMenuButtonTapMethod);
			_jumpButton.onClick.AddListener(OnJumpButtonTapMethod);
		}

		private void OnDisable()
		{
			_menuButton.onClick.RemoveListener(OnMenuButtonTapMethod);
			_jumpButton.onClick.RemoveListener(OnJumpButtonTapMethod);
		}
	}
}
