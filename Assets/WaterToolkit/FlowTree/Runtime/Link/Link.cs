using System;
using UnityEngine;

namespace WaterToolkit.FlowTrees
{
	public class Link : Node
	{
		public override string internal_description {
			get {
				return "The most basic FlowTree [Node]. " + Environment.NewLine +
					   "Calling Next() will result to the " +
					   "FlowTree moving to the [Node] right below this. If there's none " +
					   "or the next is a [LinkGroup], then the flow is finished.";
			}
		}

		public override void Next()
		{
			Node next = null;
			Transform source = transform;

			while(next == null) {
				next = GetNextNode(source);
				source = source.parent;

				bool isStop = source == tree.transform || source.TryGetComponent(out LinkGroup linkGroup);
				if(isStop) { break; }
			}

			tree.Swap(next);
		}

		private Node GetNextNode(Transform source)
		{
			Node next = null;

			int nextIndex = source.GetSiblingIndex() + 1;
			if(nextIndex < source.parent.childCount) {
				next = source.parent.GetChild(nextIndex).GetComponent<Node>();
			}

			return next;
		}
	}
}
