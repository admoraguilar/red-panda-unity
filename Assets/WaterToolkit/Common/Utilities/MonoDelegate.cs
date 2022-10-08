using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace WaterToolkit
{
	public class MonoDelegate : MonoBehaviour
	{
		private static List<MonoDelegate> _delegates = new List<MonoDelegate>();

		public static MonoDelegate GetOrCreate(string name, bool isPersistent = false)
		{
			MonoDelegate del = GetDelegate(name, isPersistent);
			if(del != null) { return del; }

			name = MakePersistentName(name);
			GameObject delGO = new GameObject(name);
			if(isPersistent) { UObject.DontDestroyOnLoad(delGO); }
			
			MonoDelegate mono = delGO.AddComponent<MonoDelegate>();
			_delegates.Add(mono);

			return mono;
		}

		public static void Destroy(string name, bool isPersistent = false)
		{
			MonoDelegate del = GetDelegate(name, isPersistent);
			_delegates.Remove(del);
			Destroy(del);
		}

		private static MonoDelegate GetDelegate(string name, bool isPersistent)
		{
			_delegates.RemoveAll(d => d == null);

			if(isPersistent) { name = MakePersistentName(name); }
			return _delegates.FirstOrDefault(d => d.name == name);
		}

		private static string MakePersistentName(string name) => $"~{name}";



		public event Action UpdateCallback = delegate { };
		public event Action FixedUpdateCallback = delegate { };
		public event Action LateUpdateCallback = delegate { };

		private void Update() => UpdateCallback();
		private void FixedUpdate() => FixedUpdateCallback();
		private void LateUpdate() => LateUpdateCallback();
	}
}
