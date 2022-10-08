using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBird
{
	public class GameOverScreen : MonoBehaviour
	{
		public event Action OnRetryButtonTap = delegate { };

		[SerializeField]
		private TMP_Text _currentScoreText = null;

		[SerializeField]
		private TMP_Text _bestScoreText = null;

		[SerializeField]
		private Button _retryButton = null;

		public void SetCurrentScoreText(int score) => _currentScoreText.text = $"{score}";

		public void SetBestScoreText(int best) => _bestScoreText.text = $"{best}";

		private void OnRetryButtonTapMethod() => OnRetryButtonTap();

		private void OnEnable()
		{
			_retryButton.onClick.AddListener(OnRetryButtonTapMethod);
		}

		private void OnDisable()
		{
			_retryButton.onClick.RemoveListener(OnRetryButtonTapMethod);
		}
	}
}
