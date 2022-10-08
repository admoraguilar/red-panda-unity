using System;
using UnityEngine;
using UnityEngine.UI;

namespace FlappyBird
{
	public class PreGameScreen : MonoBehaviour
	{
		public event Action OnPlayButtonTap = delegate { };
		public event Action OnSettingsButtonTap = delegate { };

		[SerializeField]
		private Button _playButton = null;

		[SerializeField]
		private Button _settingsButton = null;

		private void OnPlayButtonTapMethod() => OnPlayButtonTap();

		private void OnSettingsButtonTapMethod() => OnSettingsButtonTap();

		private void OnEnable()
		{
			_playButton.onClick.AddListener(OnPlayButtonTapMethod);
			_settingsButton.onClick.AddListener(OnSettingsButtonTapMethod);
		}

		private void OnDisable()
		{
			_playButton.onClick.RemoveListener(OnPlayButtonTapMethod);
			_settingsButton.onClick.RemoveListener(OnSettingsButtonTapMethod);
		}
	}
}
