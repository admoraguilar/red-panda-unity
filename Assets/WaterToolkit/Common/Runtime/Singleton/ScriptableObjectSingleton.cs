using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using UObject = UnityEngine.Object;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace WaterToolkit
{
	public abstract class ScriptableObjectSingleton<T> : ScriptableObjectSingleton where T : ScriptableObjectSingleton
	{
		private static T _instance = default;

		public static T instance => _instance;

		private void OnEnable()
		{
#if UNITY_EDITOR

			List<UObject> preloadedAssets = _ScriptableSingletonUtilities.RefreshPreloadedAssets();

#endif
			
			if(!_ScriptableSingletonUtilities.IsInstanceNull(_instance) && _instance != this) {
#if UNITY_EDITOR

				// Hack to simulate cancelling an asset creation
				EditorApplication.delayCall += () => {
					EditorWindow.focusedWindow.SendEvent(Event.KeyboardEvent("[esc]"));
					Selection.activeObject = _instance;
					DestroyImmediate(this, true);
				};

#else

				DestroyImmediate(this, true);

#endif
				return;
			}

			_instance = this as T;

#if UNITY_EDITOR

			_ScriptableSingletonUtilities.AddToPreloadedAssets(_instance);

#endif
		}

		private void OnDisable()
		{
			if(_instance == this) {
				_instance = null;
			}
		}
	}

	public abstract class ScriptableObjectSingleton : ScriptableObject { }


	class _ScriptableSingletonUtilities
	{
#if UNITY_EDITOR

		internal static void AddToPreloadedAssets(UObject instance)
		{
			List<UObject> preloadedAssets = RefreshPreloadedAssets();
			if(!IsInstanceNull(instance) && !preloadedAssets.Contains(instance)) {
				preloadedAssets.Add(instance);
				PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
			}
		}

		internal static List<UObject> RefreshPreloadedAssets()
		{
			List<UObject> preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
			preloadedAssets.RemoveAll(asset => IsInstanceNull(asset));
			PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
			return preloadedAssets;
		}

#endif

		internal static bool IsInstanceNull(UObject instance) => object.ReferenceEquals(instance, null);
	}

#if UNITY_EDITOR

	class _ScriptableSingletonEditor
	{
		[InitializeOnLoadMethod]
		private static void OnEditorInitialize()
		{
			// Touch the preloaded assets so it'll always be loaded upon the opening of the editor
			List<UObject> preloadedAssets = _ScriptableSingletonUtilities.RefreshPreloadedAssets();

			Type[] derivatives = typeof(ScriptableObjectSingleton<>).GetAllTypeDerivatives();
			foreach(Type derivative in derivatives) {
				UObject[] instances = Resources.FindObjectsOfTypeAll(derivative);
				if(instances.Length > 0) {
					_ScriptableSingletonUtilities.AddToPreloadedAssets(instances[0]); 
				}

			}

			EditorApplication.projectChanged += OnProjectChanged;
		}

		private static void OnProjectChanged()
		{
			_ScriptableSingletonUtilities.RefreshPreloadedAssets();
		}
	}

#endif
}
