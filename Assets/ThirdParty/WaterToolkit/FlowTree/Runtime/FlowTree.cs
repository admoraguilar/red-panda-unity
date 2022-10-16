using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit.FlowTrees
{
	public class FlowTree : MonoBehaviour
	{
		private static string _currentPrepend => "[Current] ";

		public event Action<Node> OnVisitNode = delegate { };
		public event Action<Node> OnLeaveNode = delegate { };
		public event Action<Node> OnResumeNode = delegate { };
		public event Action<Node> OnSuspendNode = delegate { };

		[SerializeField]
		private List<Node> _nodeStack = new List<Node>();

		[SerializeField]
		private bool _isSetRootAtStart = false;

		[SerializeField]
		private Node _root = null;

		public Node root => _root;

		public List<Node> nodeStack => _nodeStack;

		public Node current
		{
			get {
				if(_nodeStack.Count <= 0) { return null; }
				else { return _nodeStack[_nodeStack.Count - 1]; }
			}
		}

		public void Next() => RunNextFrame(() => NextNow());

		public void NextNow()
		{
			if(Equals(current, null)) { PushNow(root); }
			else { current.Next(); }
		}

		public void Set(Node node) => RunNextFrame(() => SetNow(node));

		public void SetNow(Node node)
		{
			while(nodeStack.Count > 0) { PopNow(); }
			if(node != null) { PushNow(node); }
		}

		public void Push(Node node) => RunNextFrame(() => PushNow(node));

		public void PushNow(Node node)
		{
			if(Equals(node, null)) { return; }
			if(Equals(current, node)) { return; }

			SuspendNode(current);
			if(node.isIncludeInStack) { nodeStack.Add(node); }
			VisitNode(current);
		}

		public void Pop() => RunNextFrame(() => PopNow());

		public void PopNow()
		{
			if(nodeStack.Count <= 0) { return; }

			LeaveNode(current);
			nodeStack.RemoveAt(nodeStack.Count - 1);
			ResumeNode(current);
		}

		public void PopUntilRemoved(Node node) => RunNextFrame(() => PopUntilRemovedNow(node));

		public void PopUntilRemovedNow(Node node)
		{
			if(Equals(node, null)) { return; }
			if(nodeStack.Count <= 0) { return; }
			while(nodeStack.Contains(node)) {
				PopNow();
			}
		}

		public void Swap(Node node) => RunNextFrame(() => SwapNow(node));

		public void SwapNow(Node node)
		{
			Swap(nodeStack.Count - 1, node);
		}

		internal void Swap(int index, Node node)
		{
			if(Equals(current, node)) { return; }
			if(index < 0 || nodeStack.Count <= index) { return; }

			Node toReplace = nodeStack[index];
			LeaveNode(toReplace);

			if(node != null) {
				if(node.isIncludeInStack) { nodeStack[index] = node; }
				VisitNode(node);
			} else {
				nodeStack.Clear();
			}
		}

		private void RunNextFrame(Action action)
		{
			StartCoroutine(RunNextFrame());

			IEnumerator RunNextFrame()
			{
				yield return new WaitForEndOfFrame();
				action?.Invoke();
			}
		}

		internal void VisitNode(Node node) => 
			OperateNode(node, node => node.Visit(), "Visit", true, node => OnVisitNode(node));

		internal void LeaveNode(Node node) => 
			OperateNode(node, node => node.Leave(), "Leave", false, node => OnLeaveNode(node));

		internal void ResumeNode(Node node) => 
			OperateNode(node, node => node.Resume(), "Resume", true, node => OnResumeNode(node));

		internal void SuspendNode(Node node) => 
			OperateNode(node, node => node.Suspend(), "Suspend", false, node => OnSuspendNode(node));

		private void OperateNode(Node node, Action<Node> nodeOp, string opName, bool isVisit, Action<Node> onFinish)
		{
			if(Equals(node, null)) { return; }
			nodeOp(node);

			Debug.Log($"[{typeof(FlowTree)}] {opName}: {node.name.Replace(_currentPrepend, string.Empty)}");
			if(isVisit) { node.name = current.name.Insert(0, _currentPrepend); }
			else { node.name = current.name.Replace(_currentPrepend, string.Empty); }
			
			onFinish(node);
		}

		private void Start()
		{
			if(_isSetRootAtStart) { Next(); }
		}

		private void OnDestroy()
		{
			SetNow(null);
		}
	}
}
