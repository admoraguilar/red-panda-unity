using System;

namespace WaterToolkit.FlowTrees
{
	public class LinkGroup : Link
	{
		public override string internal_description
		{
			get {
				return "A [Link] Node." + Environment.NewLine +
					   "Mostly useful for grouping [Link] nodes together. Calling Next() will " +
					   "result to the FlowTree moving immediately to it's first child, if there's no " +
					   "chilren however then this is skipped. If a FlowTree also reaches the last [Link] node " +
					   "for this group, the flow is finished.";
			}
		}

		public override bool isIncludeInStack => false;

		protected override void OnDoVisit()
		{
			Next();
		}

		public override void Next()
		{
			if(transform.childCount == 0) {
				return;
			}

			Node next = transform.GetChild(0).GetComponent<Node>();
			tree.Swap(next);
		}
	}
}
