using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit.Blackboards
{
	public class Blackboard
	{
		private class ResolverInfo
		{
			public object source;
			public Action resolver;
			public Func<bool> resolveIf;
		}

		private List<object> _dataList = new List<object>();
		private List<ResolverInfo> _resolvables = new List<ResolverInfo>();

		public void AddResolvable(object source, Action resolver) => AddResolvable(source, null, resolver);

		public void AddResolvable(object source, Func<bool> resolveIf, Action resolver)
		{
			RemoveResolvableNulls();
			_resolvables.Add(new ResolverInfo { 
				source = source, 
				resolveIf = resolveIf, 
				resolver = resolver });

			bool shouldResolve = true;
			if(resolveIf != null) { shouldResolve = resolveIf(); }
			if(shouldResolve) { resolver(); }
		}

		public void RemoveResolvable(object source)
		{
			RemoveResolvableNulls();
			_resolvables.RemoveAll(r => Equals(r, source));
		}

		private void RemoveResolvableNulls()
		{
			_resolvables.RemoveAll(r => {
				if(r.source is UnityEngine.Object uObj) { return uObj == null; }
				else { return r.source == null; }
			});
		}

		internal void InternalUpdate()
		{
			RemoveResolvableNulls();
			foreach(ResolverInfo info in _resolvables) {
				bool shouldResolve = true;
				if(info.resolveIf != null) { shouldResolve = info.resolveIf(); }
				if(shouldResolve) { info.resolver(); }
			}
		}

		public T GetNearest<T>(GameObject source, Predicate<T> match) where T : Component
		{
			T result = default;
			if(result == null) { result = GetFromSibling(source, match); }
			if(result == null) { result = GetFromParent(source, match); }
			if(result == null) { result = GetFromSameScene(source, match); }
			if(result == null) { result = Get(match); }
			return result;
		}

		public T GetFromSameScene<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetManyFromSameScene(source, match).FirstOrDefault();
		}

		public IEnumerable<T> GetManyFromSameScene<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetMany<T>(item => item.gameObject.scene.handle == source.scene.handle && match(item));
		}

		public T GetFromParent<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetManyFromParent(source, match).FirstOrDefault();
		}

		public IEnumerable<T> GetManyFromParent<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetMany<T>(item => source.transform.IsChildOf(item.transform) && match(item));
		}

		public T GetFromSibling<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetManyFromSibling(source, match).FirstOrDefault();
		}

		public IEnumerable<T> GetManyFromSibling<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetMany<T>(item => item.transform.parent == source.transform.parent && match(item));
		}

		public T GetFromChild<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetManyFromChild(source, match).FirstOrDefault();
		}

		public IEnumerable<T> GetManyFromChild<T>(GameObject source, Predicate<T> match) where T : Component
		{
			return GetMany<T>(item => item.transform.IsChildOf(source.transform) && match(item));
		}

		public T Get<T>()
		{
			RemoveNulls();
			return InternalGet<T>();
		}

		public T Get<T>(Predicate<T> match)
		{
			RemoveNulls();
			return InternalGet(match);
		}

		public IEnumerable<T> GetMany<T>(Predicate<T> match)
		{
			RemoveNulls();
			return InternalGetMany(match);
		}

		public void PushRange<T>(IEnumerable<T> collection)
		{
			RemoveNulls();
			InternalPushRange(collection);
		}

		public void Push<T>(T data)
		{
			RemoveNulls();
			InternalPush(data);
		}

		public void RemoveRange<T>(IEnumerable<T> collection)
		{
			RemoveNulls();
			InternalRemoveRange(collection);
		}

		public void Remove<T>(T data)
		{
			RemoveNulls();
			InternalRemove(data);
		}

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

	public static class SBlackboard
	{
		public static readonly Blackboard instance = new Blackboard();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitOnLoad()
		{
			MonoDelegate del = MonoDelegate.GetOrCreate(nameof(SBlackboard), true);
			del.UpdateCallback += () => { instance.InternalUpdate(); };
		}

		public static void AddResolvable(object source, Action resolver) =>
			instance.AddResolvable(source, resolver);

		public static void AddResolvable(object source, Func<bool> resolveIf, Action resolver) =>
			instance.AddResolvable(source, resolveIf, resolver);

		public static void RemoveResolvable(object source) =>
			instance.RemoveResolvable(source);

		public static T GetNearest<T>(GameObject source, Predicate<T> match) where T : Component => 
			instance.GetNearest(source, match);

		public static T GetFromSameScene<T>(GameObject source, Predicate<T> match) where T : Component => 
			instance.GetFromSameScene(source, match);

		public static IEnumerable<T> GetManyFromSameScene<T>(GameObject source, Predicate<T> match) where T : Component =>
			instance.GetManyFromSameScene(source, match);

		public static T GetFromParent<T>(GameObject source, Predicate<T> match) where T : Component =>
			instance.GetFromParent(source, match);

		public static IEnumerable<T> GetManyFromParent<T>(GameObject source, Predicate<T> match) where T : Component =>
			instance.GetManyFromParent(source, match);

		public static T GetFromSibling<T>(GameObject source, Predicate<T> match) where T : Component =>
			instance.GetFromSibling(source, match);

		public static IEnumerable<T> GetManyFromSibling<T>(GameObject source, Predicate<T> match) where T : Component =>
			instance.GetManyFromSibling(source, match);

		public static T GetFromChild<T>(GameObject source, Predicate<T> match) where T : Component =>
			instance.GetFromChild(source, match);

		public static IEnumerable<T> GetManyFromChild<T>(GameObject source, Predicate<T> match) where T : Component =>
			instance.GetManyFromChild(source, match);

		public static T Get<T>() => instance.Get<T>();

		public static T Get<T>(Predicate<T> match) => instance.Get(match);

		public static IEnumerable<T> GetMany<T>(Predicate<T> match) => instance.GetMany(match);

		public static void PushRange<T>(IEnumerable<T> collection) => instance.PushRange(collection);

		public static void Push<T>(T data) => instance.Push(data);

		public static void RemoveRange<T>(IEnumerable<T> collection) => instance.RemoveRange(collection);

		public static void Remove<T>(T data) => instance.Remove(data);
	}
}
