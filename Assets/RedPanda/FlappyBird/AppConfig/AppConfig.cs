using UnityEngine;

namespace FlappyBird
{
	public class AppConfig : MonoBehaviour
	{
		[SerializeField]
		private int _targetFrameRate = 60;

		public int targetFrameRate
		{
			get => _targetFrameRate;
			set => SetTargetFramerate(value);
		}

		private void SetTargetFramerate(int value, bool forceSet = false)
		{
			if(_targetFrameRate == value && !forceSet) { return; }
			_targetFrameRate = value;
			Application.targetFrameRate = _targetFrameRate;
		}

		private void Start()
		{
			SetTargetFramerate(targetFrameRate, true);
		}
	}
}
