using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit
{
	public static class GameObjectExtensions
	{
		public static T GetCachedComponent<T>(this GameObject gameObject, ref T cache)
		{
			if(cache == null) { cache = gameObject.GetComponent<T>(); }
			return cache;
		}

		public static IList<T> GetComponentsInParentAndChildren<T>(this GameObject gameObject, bool includeInactive = false)
		{
			List<T> results = new List<T>();
			results.AddRange(gameObject.GetComponentsInParent<T>(includeInactive));
			results.AddRange(gameObject.GetComponentsInChildren<T>(includeInactive));
			return results;
		}

		public static T GetComponentInParentAndChildren<T>(this GameObject gameObject, bool includeInactive = false)
		{
			T result = gameObject.GetComponentInParent<T>(includeInactive);
			if(result == null) { result = gameObject.GetComponentInChildren<T>(includeInactive); }
			return result;
		}
	}
}
