using UnityEngine;

namespace FlappyBird
{
	public class DefaultUI : MonoBehaviour
	{
		public enum Mode
		{
			PreGame,
			InGame,
			GameOver
		}

		[SerializeField]
		private PreGameScreen _preGameScreen = null;

		[SerializeField]
		private InGameScreen _inGameScreen = null;

		[SerializeField]
		private GameOverScreen _gameOverScreen = null;

		private Mode _mode = default;

		public Mode mode
		{
			get => _mode;
			private set => _mode = value;
		}

		public PreGameScreen preGameScreen => _preGameScreen;
		
		public InGameScreen inGameScreen => _inGameScreen;

		public GameOverScreen gameOverScreen => _gameOverScreen;

		public void To(Mode mode)
		{
			this.mode = mode;

			_preGameScreen.gameObject.SetActive(this.mode == Mode.PreGame);
			_inGameScreen.gameObject.SetActive(this.mode == Mode.InGame);
			_gameOverScreen.gameObject.SetActive(this.mode == Mode.GameOver);
		}

		private void Start()
		{
			_preGameScreen.gameObject.SetActive(false);
			_inGameScreen.gameObject.SetActive(false);
			_gameOverScreen.gameObject.SetActive(false);
		}
	}
}
