using UnityEngine;

namespace WaterToolkit
{
	[CreateAssetMenu(menuName = "Water Toolkit/App Config")]
	public class AppConfig : ScriptableObjectSingleton<AppConfig>
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RunOnInitLoad()
		{
			instance.Initialize();
		}

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

		private void Initialize()
		{
			SetTargetFramerate(targetFrameRate, true);
		}
	}

	public static class SAppConfig
	{
		public static AppConfig instance => AppConfig.instance;

		public static int targetFrameRate
		{
			get => instance.targetFrameRate;
			set => instance.targetFrameRate = value;
		}
	}
}
