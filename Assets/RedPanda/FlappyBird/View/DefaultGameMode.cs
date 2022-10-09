using System;
using UnityEngine;
using UnityEngine.Rendering;
using WaterToolkit;

namespace FlappyBird
{
	public class DefaultGameMode : MonoBehaviour
	{
		public enum GamePhase
		{
			None,
			GameInit,
			InGame,
			GameOver
		};

		public event Action OnInitGame = delegate { };
		public event Action OnStartGame = delegate { };
		public event Action OnStopGame = delegate { };
		public event Action OnBirdCollideOnPipe = delegate { };
		public event Action OnBirdPassThruPipe = delegate { };
		public event Action OnBirdCollideOnGround = delegate { };
		public event Action OnBirdJump = delegate { };

		[SerializeField]
		private BirdCharacter _birdPrefab = null;

		[SerializeField]
		private LevelGenerator _levelGenerator = null;

		private BirdCharacter _birdInstance = null;
		private GamePhase _gamePhase = default;

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

		public GamePhase gamePhase
		{
			get => _gamePhase;
			private set => _gamePhase = value;
		}

		public void InitializePreGame()
		{
			if(gamePhase == GamePhase.GameInit) { return; }
			gamePhase = GamePhase.GameInit;

			
		}

		public void StartGame()
		{
			if(gamePhase == GamePhase.None || gamePhase == GamePhase.GameOver) {
				gamePhase = GamePhase.GameInit;

				birdInstance = Instantiate(_birdPrefab);
				birdInstance.rigidbody2D.simulated = false;

				birdInstance.OnCollisionEnter2DCallback += (collision) => {
					MultiTags tags = collision.collider.GetComponent<MultiTags>();
					if(tags.Contains("PipePart")) { OnBirdCollideOnPipe(); }
					else if(tags.Contains("Ground")) { OnBirdCollideOnGround(); }
				};

				birdInstance.OnTriggerEnter2DCallback += (collider) => {
					MultiTags tags = collider.GetComponent<MultiTags>();
					if(tags.Contains("PipePart")) { OnBirdCollideOnPipe(); }
					else if(tags.Contains("PipeSpace")) { OnBirdPassThruPipe(); }
					else if(tags.Contains("Ground")) { OnBirdCollideOnGround(); }
				};

				birdInstance.OnJump += () => { 
					if(gamePhase == GamePhase.GameInit) { StartGame(); }
					OnBirdJump(); 
				};

				OnInitGame();
			} else if(gamePhase == GamePhase.GameInit) {
				gamePhase = GamePhase.InGame;

				if(birdInstance != null) { birdInstance.rigidbody2D.simulated = true; }
				levelGenerator.StartGenerate();

				OnStartGame();
			}
		}

		public void StopGame()
		{
			if(gamePhase == GamePhase.GameOver) { return; }
			gamePhase = GamePhase.GameOver;

			levelGenerator.StopMovement();

			OnStopGame();
		}

		public void CleanupGame()
		{
			if(_birdInstance != null) { Destroy(birdInstance.gameObject); }
			levelGenerator.StopGenerate();
		}
	}
}
