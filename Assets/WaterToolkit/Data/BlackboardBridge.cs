using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit.Data
{
	[DefaultExecutionOrder(ExecutionOrderUtilities.CORE_SYSTEMS)]
	public class BlackboardBridge : MonoBehaviour
	{
		public bool shouldGetChildComponents = false;

		private List<Component> _components = new List<Component>();

		private void Awake()
		{
			_components.Add(this);
			_components.AddRange(GetComponentsInChildren<Component>(true));

			Blackboard.PushRange(_components);
		}

		private void OnDestroy()
		{
			Blackboard.RemoveRange(_components);
		}
	}
}
