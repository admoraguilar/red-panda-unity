using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WaterToolkit
{
	public abstract class ScriptableScene : ScriptableObject
	{
		public event Action OnSceneLoaded = delegate { };
		public event Action OnSceneUnloaded = delegate { };

		public int index = 0;

		public Scene scene => SceneManager.GetSceneByBuildIndex(index);

		public virtual AsyncOperation LoadAsync(LoadSceneParameters parameters)
		{
			AsyncOperation loadOp = SceneManager.LoadSceneAsync(index, parameters);
			loadOp.allowSceneActivation = true;
			loadOp.completed += _ => OnSceneLoaded();
			return loadOp;
		}

		public virtual AsyncOperation UnloadAsync()
		{
			AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(index);
			unloadOp.allowSceneActivation = true;
			unloadOp.completed += _ => OnSceneUnloaded();
			return unloadOp;
		}

		private class CustomAsync : AsyncOperation
		{
			
		}
	}
}
