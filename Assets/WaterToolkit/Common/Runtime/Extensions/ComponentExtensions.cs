using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit
{
    public static class ComponentExtensions
    {
        public static T GetCachedComponent<T>(this Component component, ref T cache)
		{
            if(cache == null) { cache = component.GetComponent<T>(); }
            return cache;
		}

		public static IList<T> GetComponentsInParentAndChildren<T>(this Component component, bool includeInactive = false)
		{
			List<T> results = new List<T>();
			results.AddRange(component.GetComponentsInParent<T>(includeInactive));
			results.AddRange(component.GetComponentsInChildren<T>(includeInactive));
			return results;
		}

		public static T GetComponentInParentAndChildren<T>(this Component component, bool includeInactive = false)
		{
			T result = component.GetComponentInParent<T>(includeInactive);
			if(result == null) { result = component.GetComponentInChildren<T>(includeInactive); }
			return result;
		}
    }
}
