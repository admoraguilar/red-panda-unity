using System;
using UnityEngine;
using WaterToolkit.Blackboards;

namespace FlappyBird
{
	public class GameAudioLogic : MonoBehaviour
	{
		private AudioPlayer _audioPlayer = null;
		private FlowDirectoryData _flowDirectoryData = null;
		private DefaultUI _ui = null;
		private DefaultGameMode _gameMode = null;

		private void OnPlayButtonTapMethod()
		{
			_audioPlayer.PlaySwooshSFX();
		}

		private void OnBirdCollideOnGroundMethod()
		{
			if(_gameMode.gamePhase == DefaultGameMode.GamePhase.GameOver) { return; }
			_audioPlayer.PlayHitSFX();
		}

		private void OnBirdCollideOnPipeMethod()
		{
			if(_gameMode.gamePhase == DefaultGameMode.GamePhase.GameOver) { return; }
			_audioPlayer.PlayHitSFX();
		}

		private void OnBirdPassThruPipe()
		{
			if(_gameMode.gamePhase == DefaultGameMode.GamePhase.GameOver) { return; }
			_audioPlayer.PlayPointSFX();
		}

		private void OnBirdJump()
		{
			_audioPlayer.PlayWingFlapSFX();
		}

		private void Awake()
		{
			_audioPlayer = SBlackboard.Get<AudioPlayer>();
			_flowDirectoryData = SBlackboard.Get<FlowDirectoryData>();
			_ui = SBlackboard.Get<DefaultUI>();
			_gameMode = SBlackboard.Get<DefaultGameMode>();
		}

		private void OnEnable()
		{
			_ui.preGameScreen.OnPlayButtonTap += OnPlayButtonTapMethod;
			_gameMode.OnBirdCollideOnGround += OnBirdCollideOnGroundMethod;
			_gameMode.OnBirdCollideOnPipe += OnBirdCollideOnPipeMethod;
			_gameMode.OnBirdPassThruPipe += OnBirdPassThruPipe;
			_gameMode.OnBirdJump += OnBirdJump;
		}

		private void OnDisable()
		{
			_ui.preGameScreen.OnPlayButtonTap += OnPlayButtonTapMethod;
			_gameMode.OnBirdCollideOnGround -= OnBirdCollideOnGroundMethod;
			_gameMode.OnBirdCollideOnPipe -= OnBirdCollideOnPipeMethod;
			_gameMode.OnBirdPassThruPipe -= OnBirdPassThruPipe;
			_gameMode.OnBirdJump -= OnBirdJump;
		}
	}
}
