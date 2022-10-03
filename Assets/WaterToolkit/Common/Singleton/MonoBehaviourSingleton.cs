using UnityEngine;

namespace WaterToolkit
{
	public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance
		{
			get {
				if(_instance == null) {
					GameObject instanceGo = new GameObject(nameof(T));
					_instance = instanceGo.AddComponent<T>();
				}
				return _instance;
			}
		}
		private static T _instance = default;

		private void OnEnable()
		{
			if(_instance != null && _instance != this) {
				Destroy(this);
				return;
			}

			_instance = this as T;
		}

		private void OnDisable()
		{
			if(_instance == this) {
				_instance = null;
			}
		}
	}
}
