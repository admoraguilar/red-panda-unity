using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UObject = UnityEngine.Object;

namespace FlappyBird
{
	public class GameData
	{
		public enum Flow
		{
			Menu,
			Game,
			GameOver
		}

		public event Action OnFlowChange = delegate { };

		private List<string> _fullFlow = new List<string>() {
			Flow.Menu.ToString(),
		};

		public string flow => _fullFlow[_fullFlow.Count - 1];

		public void Push(string flow)
		{
			_fullFlow.Add(flow);
			OnFlowChange();
		}

		public void Pop(string flow)
		{
			if(_fullFlow.Count <= 0) { return; }
			_fullFlow.RemoveAt(_fullFlow.Count - 1);
			OnFlowChange();
		}

		public void Replace(string flow)
		{
			if(_fullFlow.Count <= 0) { return; }
			_fullFlow[_fullFlow.Count - 1] = flow;
			OnFlowChange();
		}

		public void Set(string flow)
		{
			_fullFlow.Clear();
			_fullFlow.Add(flow);
			OnFlowChange();
		}
	}

	public class GameController : MonoBehaviour
	{
		// Instantiate bird

		// On Start
		// Enable Level Generator
		// Hook controller

		[SerializeField]
		private GameplayLogic _gameplayLogic = null;

		[SerializeField]
		private UILogic _uiLogic = null;		

		private void Awake()
		{
			FlappyData.instance.Deserialize();

			FlappyData.instance.Get<GameData>().Set(GameData.Flow.Game.ToString());

			_gameplayLogic.Awake();
			_uiLogic.Awake();
		}

		private void Start()
		{
			_gameplayLogic.Start();
			_uiLogic.Start();
		}

		private void OnEnable()
		{
			_gameplayLogic.OnEnable();
			_uiLogic.OnEnable();
		}

		private void OnDisable()
		{
			_gameplayLogic.OnDisable();
			_uiLogic.OnDisable();
		}
	}

	[Serializable]
	public class GameplayLogic
	{
		public BirdCharacter birdPrefab = null;
		public LevelGenerator levelGenerator = null;

		private GameData _gameData = null;

		private BirdCharacter _birdInstance = null;

		private void OnGameDataFlowChanged()
		{
			if(_gameData.flow == GameData.Flow.Game.ToString()) {
				_birdInstance = UObject.Instantiate(birdPrefab);
				_birdInstance.OnUpdateCallback += () => {
					if(Input.GetKeyDown(KeyCode.F)) {
						_birdInstance.Jump();
						Debug.Log("Jump");
					}
				};
				_birdInstance.OnTriggerEnter2DCallback += (collider) => {
					Debug.Log($"Collider name = {collider.name}");

					if(collider.name == "Square") {
						_gameData.Set(GameData.Flow.GameOver.ToString());
					}
				};
				levelGenerator.StartGenerate();
			} else if(_gameData.flow == GameData.Flow.GameOver.ToString()) {
				if(_birdInstance != null) {
					UObject.Destroy(_birdInstance.gameObject);
				}
				
				levelGenerator.StopGenerate();
			}
		}

		public void Awake()
		{
			_gameData = FlappyData.instance.Get<GameData>();
		}

		public void Start()
		{
			OnGameDataFlowChanged();
		}

		public void OnEnable()
		{
			_gameData.OnFlowChange += OnGameDataFlowChanged;
		}

		public void OnDisable()
		{
			_gameData.OnFlowChange -= OnGameDataFlowChanged;
		}
	}

	[Serializable]
	public class UILogic
	{
		public TMP_Text _flowText = null;
		public Button startButton = null;

		private GameData _gameData = null;

		private void OnGameDataFlowChanged()
		{
			_flowText.text = _gameData.flow;

			TMP_Text text = startButton.GetComponentInChildren<TMP_Text>();
			if(_gameData.flow == GameData.Flow.Menu.ToString()) {
				text.text = "Play";
			} else if(_gameData.flow == GameData.Flow.GameOver.ToString()) {
				text.text = "Retry";
			}

			if(_gameData.flow == GameData.Flow.Game.ToString()) {
				startButton.gameObject.SetActive(false);
			} else {
				startButton.gameObject.SetActive(true);
			}
		}

		private void OnStartButtonClick()
		{
			if(_gameData.flow == GameData.Flow.Menu.ToString()) {
				_gameData.Set(GameData.Flow.Game.ToString());
			} else if(_gameData.flow == GameData.Flow.GameOver.ToString()) {
				_gameData.Set(GameData.Flow.Menu.ToString());
			}
		}

		public void Awake()
		{
			_gameData = FlappyData.instance.Get<GameData>();
		}

		public void Start()
		{
			OnGameDataFlowChanged();
		}

		public void OnEnable()
		{
			_gameData.OnFlowChange += OnGameDataFlowChanged;
			startButton.onClick.AddListener(OnStartButtonClick);
		}

		public void OnDisable()
		{
			_gameData.OnFlowChange -= OnGameDataFlowChanged;
			startButton.onClick.RemoveListener(OnStartButtonClick);
		}
	}
}
