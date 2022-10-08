using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit.Data
{
	public class Blackboard
	{
		public static readonly Blackboard instance = new Blackboard();

		public static T GetNearest<T>(GameObject source, Predicate<T> match) where T : Component
		{
			T result = default;
			if(result == null) { result = GetFromSibling(source, match); }
			if(result == null) { result = GetFromParent(source, match); }
			if(result == null) { result = GetFromSameScene(source, match); }
			if(result == null) { result = Get(match); }
			return result;
		}

		public static T GetFromSameScene<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetManyFromSameScene(source, match).FirstOrDefault();
		}

		public static IEnumerable<T> GetManyFromSameScene<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetMany<T>(item => item.gameObject.scene.handle == source.scene.handle && match(item));
		}

		public static T GetFromParent<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetManyFromParent(source, match).FirstOrDefault();
		}

		public static IEnumerable<T> GetManyFromParent<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetMany<T>(item => source.transform.IsChildOf(item.transform) && match(item));
		}

		public static T GetFromSibling<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetManyFromSibling(source, match).FirstOrDefault();
		}

		public static IEnumerable<T> GetManyFromSibling<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetMany<T>(item => item.transform.parent == source.transform.parent && match(item));
		}

		public static T GetFromChild<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetManyFromChild(source, match).FirstOrDefault();
		}

		public static IEnumerable<T> GetManyFromChild<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetMany<T>(item => item.transform.IsChildOf(source.transform) && match(item));
		}

		public static T Get<T>()
		{
			instance.RemoveNulls();
			return instance.InternalGet<T>();
		}

		public static T Get<T>(Predicate<T> match)
		{
			instance.RemoveNulls();
			return instance.InternalGet(match);
		}

		public static IEnumerable<T> GetMany<T>(Predicate<T> match)
		{
			instance.RemoveNulls();
			return instance.InternalGetMany(match);
		}

		public static void PushRange<T>(IEnumerable<T> collection)
		{
			instance.RemoveNulls();
			instance.InternalPushRange(collection);
		}

		public static void Push<T>(T data)
		{
			instance.RemoveNulls();
			instance.InternalPush(data);
		}

		public static void RemoveRange<T>(IEnumerable<T> collection)
		{
			instance.RemoveNulls();
			instance.InternalRemoveRange(collection);
		}

		public static void Remove<T>(T data)
		{
			instance.RemoveNulls();
			instance.InternalRemove(data);
		}



		private List<object> _dataList = new List<object>();

		private T InternalGet<T>()
		{
			return InternalGet<T>(d => d is T);
		}

		private T InternalGet<T>(Predicate<T> match)
		{
			IEnumerable<T> results = InternalGetMany(match);
			if(results.Count() < 0) { return default; }
			else { return results.FirstOrDefault(); }
		}

		private IEnumerable<T> InternalGetMany<T>(Predicate<T> match)
		{
			return _dataList.Where(d => d is T).Cast<T>().Where(p => match(p));
		}

		private void InternalPushRange<T>(IEnumerable<T> collection)
		{
			foreach(T item in collection) { InternalPush(item); }
		}

		private void InternalPush<T>(T data)
		{
			if(_dataList.Contains(data)) { return; }
			_dataList.Add(data);
		}

		private void InternalRemoveRange<T>(IEnumerable<T> collection)
		{
			foreach(T item in collection) { InternalRemove(item); }
		}

		private void InternalRemove<T>(T data)
		{
			_dataList.Remove(data);
		}

		private void RemoveNulls()
		{
			_dataList.RemoveAll(d => d == null);
		}
	}
}
