
namespace WaterToolkit.FlowTrees
{
	public abstract class Branch : Link
	{
		public override bool isIncludeInStack => false;

		public abstract bool ConditionResult();

		protected override void OnDoVisit()
		{
			Next();
		}

		public override void Next()
		{
			if(!ConditionResult() || transform.childCount == 0) {
				base.Next();
				return;
			}

			Node next = transform.GetChild(0).GetComponent<Node>();
			tree.Swap(next);
 		}
	}
}
