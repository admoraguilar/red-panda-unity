using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit.Blackboards
{
	[DefaultExecutionOrder(ExecutionOrderUtilities.CORE_SYSTEMS)]
	public class BlackboardMonoBridge : MonoBehaviour
	{
		public bool shouldGetChildComponents = false;

		private List<Component> _components = new List<Component>();

		private void Awake()
		{
			_components.Add(this);
			_components.AddRange(GetComponentsInChildren<Component>(true));

			SBlackboard.PushRange(_components);
		}

		private void OnDestroy()
		{
			SBlackboard.RemoveRange(_components);
		}
	}
}
