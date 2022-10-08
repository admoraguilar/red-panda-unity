using System;
using UnityEngine;

namespace FlappyBird
{
	public class DefaultGameMode : MonoBehaviour
	{
		public event Action OnStartGame = delegate { };
		public event Action OnStopGame = delegate { };
		public event Action OnBirdCollideOnPipe = delegate { };

		[SerializeField]
		private BirdCharacter _birdPrefab = null;

		[SerializeField]
		private LevelGenerator _levelGenerator = null;

		private BirdCharacter _birdInstance = null;

		public BirdCharacter birdInstance
		{
			get => _birdInstance;
			private set => _birdInstance = value;
		}

		public LevelGenerator levelGenerator
		{
			get => _levelGenerator;
			private set => _levelGenerator = value;
		}

		public void StartGame()
		{
			birdInstance = Instantiate(_birdPrefab);
			birdInstance.OnTriggerEnter2DCallback += (collider) => {
				if(collider.name == "Square") { OnBirdCollideOnPipe(); }
			};
			levelGenerator.StartGenerate();

			OnStartGame();
		}

		public void StopGame()
		{
			if(_birdInstance != null) { Destroy(birdInstance.gameObject); }
			levelGenerator.StopGenerate();

			OnStopGame();
		}
	}
}
