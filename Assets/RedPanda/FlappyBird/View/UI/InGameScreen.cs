using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FlappyBird
{
	public class InGameScreen : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _currentScoreText = null;

		public void SetCurrentScore(int score)
		{
			_currentScoreText.text = $"{score}";
		}
	}
}
