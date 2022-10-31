using System;
using UnityEngine;
using WaterToolkit;
using WaterToolkit.Effects;

namespace FlappyBird
{
	public class DefaultGameMode : MonoBehaviour
	{
		public enum GamePhase
		{
			None,
			PreGame,
			InGamePhase1,
			InGamePhase2,
			GameOver
		};

		public event Action OnBirdCollideOnPipe = delegate { };
		public event Action OnBirdPassThruPipe = delegate { };
		public event Action OnBirdCollideOnGround = delegate { };
		public event Action OnBirdJump = delegate { };

		[SerializeField]
		private BirdCharacter _birdPrefab = null;

		[SerializeField]
		private LevelGenerator _levelGenerator = null;

		[SerializeField]
		private MaterialTilingScroller _groundScroller = null;

		private BirdCharacter _birdInstance = null;
		private GamePhase _gamePhase = default;

		public LevelGenerator levelGenerator => _levelGenerator;

		public MaterialTilingScroller groundScroller => _groundScroller;

		public BirdCharacter birdInstance
		{
			get => _birdInstance;
			private set => _birdInstance = value;
		}


		public GamePhase gamePhase
		{
			get => _gamePhase;
			private set => _gamePhase = value;
		}

		public bool isInGame => gamePhase == GamePhase.InGamePhase1 || gamePhase == GamePhase.InGamePhase2;

		public void PreGame()
		{
			if(gamePhase == GamePhase.PreGame) { return; }
			gamePhase = GamePhase.PreGame;

			if(groundScroller != null) { groundScroller.shouldScroll = true; }
		}

		public void StartGame()
		{
			if(groundScroller != null) { groundScroller.shouldScroll = true; }

			if(gamePhase != GamePhase.InGamePhase1 && !isInGame) {
				gamePhase = GamePhase.InGamePhase1;

				birdInstance = Instantiate(_birdPrefab);
				birdInstance.rigidbody2D.simulated = false;

				birdInstance.OnCollisionEnter2DCallback += (collision) => {
					MultiTags tags = collision.collider.GetComponent<MultiTags>();
					if(tags == null) { return; }

					if(tags.Contains("PipePart")) { OnBirdCollideOnPipe(); }
					else if(tags.Contains("Ground")) { OnBirdCollideOnGround(); }
				};

				birdInstance.OnTriggerEnter2DCallback += (collider) => {
					MultiTags tags = collider.GetComponent<MultiTags>();
					if(tags == null) { return; }

					if(tags.Contains("PipePart")) { OnBirdCollideOnPipe(); }
					else if(tags.Contains("PipeSpace")) { OnBirdPassThruPipe(); }
					else if(tags.Contains("Ground")) { OnBirdCollideOnGround(); }
				};

				birdInstance.OnJump += () => { 
					if(gamePhase == GamePhase.InGamePhase1) { StartGame(); }
					OnBirdJump(); 
				};

			} else if(gamePhase != GamePhase.InGamePhase2 && isInGame) {
				gamePhase = GamePhase.InGamePhase2;

				if(birdInstance != null) { birdInstance.rigidbody2D.simulated = true; }
				levelGenerator.StartGenerate();
			}
		}

		public void StopGame()
		{
			if(gamePhase == GamePhase.GameOver) { return; }
			gamePhase = GamePhase.GameOver;

			if(birdInstance != null) { birdInstance.Hit(); }

			levelGenerator.shouldMove = false;
			if(groundScroller != null) { groundScroller.shouldScroll = false; }
		}

		public void CleanupGame()
		{
			if(_birdInstance != null) { Destroy(birdInstance.gameObject); }
			levelGenerator.StopGenerate();
			if(groundScroller != null) { groundScroller.shouldScroll = true; }
		}
	}
}
