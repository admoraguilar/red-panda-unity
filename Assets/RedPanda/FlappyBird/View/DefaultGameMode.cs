using System;
using UnityEngine;
using WaterToolkit;

namespace FlappyBird
{
	public class DefaultGameMode : MonoBehaviour
	{
		public event Action OnStartGame = delegate { };
		public event Action OnStopGame = delegate { };
		public event Action OnBirdCollideOnPipe = delegate { };
		public event Action OnBirdPassThruPipe = delegate { };

		[SerializeField]
		private BirdCharacter _birdPrefab = null;

		[SerializeField]
		private LevelGenerator _levelGenerator = null;

		private BirdCharacter _birdInstance = null;
		private bool _isInGame = false;

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

		public bool isInGame
		{
			get => _isInGame;
			private set => _isInGame = value;
		}

		public void StartGame()
		{
			if(isInGame) { return; }
			isInGame = true;

			birdInstance = Instantiate(_birdPrefab);
			birdInstance.OnTriggerEnter2DCallback += (collider) => {
				MultiTags tags = collider.GetComponent<MultiTags>();
				if(tags.Contains("PipePart")) { OnBirdCollideOnPipe(); }
				else if(tags.Contains("PipeSpace")) { OnBirdPassThruPipe(); }
			};
			levelGenerator.StartGenerate();

			OnStartGame();
		}

		public void StopGame()
		{
			if(!isInGame) { return; }
			isInGame = false;

			if(_birdInstance != null) { Destroy(birdInstance.gameObject); }
			levelGenerator.StopGenerate();

			OnStopGame();
		}
	}
}
