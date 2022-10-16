using System;
using System.Security.Cryptography;
using UnityEngine;

namespace WaterToolkit.FlowTrees
{
	public abstract class Node : MonoBehaviour
	{
		public virtual string internal_description => string.Empty;

		public event Action OnVisit = delegate { };
		public event Action OnLeave = delegate { };
		public event Action OnResume = delegate { };
		public event Action OnSuspend = delegate { };

		public bool shouldHandleGameObjectActive = true;

		private FlowTree _tree = null;

		public FlowTree tree
		{
			get => _tree;
			set => _tree = value;
		}

		public virtual bool isIncludeInStack => true;

		public void Initialize(FlowTree tree)
		{
			_tree = tree;
			SetActive(false);
		}

		public virtual void Next() { }

		public void NextNow() => tree.NextNow();

		public void Set() => tree.Set(this);

		public void SetNow() => tree.SetNow(this);

		public void Push() => tree.Push(this);

		public void PushNow() => tree.PushNow(this);

		public void Pop() => tree.Pop();

		public void PopNow() => tree.PopNow();

		public void PopUntilRemoved() => tree.PopUntilRemoved(this);

		public void PopUntilRemovedNow() => tree.PopUntilRemovedNow(this);

		public void Swap() => tree.Swap(this);

		public void SwapNow() => tree.SwapNow(this);

		protected virtual void OnDoVisit() { }
		protected virtual void OnDoLeave() { }
		protected virtual void OnDoResume() { }
		protected virtual void OnDoSuspend() { }

		internal void Visit()
		{
			SetActive(true);
			OnDoVisit();
			OnVisit();
		}

		internal void Leave()
		{
			SetActive(false);
			OnDoLeave();
			OnLeave();
		}

		internal void Resume()
		{
			OnDoResume();
			OnResume();
		}

		internal void Suspend()
		{
			OnDoSuspend();
			OnSuspend();
		}

		private void SetActive(bool value)
		{
			if(shouldHandleGameObjectActive) { gameObject.SetActive(value); } 
			else { enabled = value; }
		}

		private void Awake()
		{
			tree = GetComponentInParent<FlowTree>();
			if(tree != null) { Initialize(tree); }
		}
	}
}
