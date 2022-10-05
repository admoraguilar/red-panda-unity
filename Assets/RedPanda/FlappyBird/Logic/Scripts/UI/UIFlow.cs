using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
	public class UIFlow : MonoBehaviour
	{
		private FlowData flowData = null;

		private void OnUIFlowChanged()
		{
			Debug.Log($"Response from {GetType()}: UI Flow changed to - [{flowData.uiFlow}].");
		}

		private void Awake()
		{
			flowData = FlappyData.instance.Get<FlowData>();
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.S)) {
				flowData.uiFlow = $"StartMenu.{Time.time:00.00}";
			}

			if(Input.GetKeyDown(KeyCode.D)) {
				Debug.Log($"Response from {GetType()}: UI Flow changed to - [{flowData.uiFlow}].");
			}
		}

		private void OnEnable()
		{
			flowData.OnUIFlowChanged += OnUIFlowChanged;
		}

		private void OnDisable()
		{
			flowData.OnUIFlowChanged -= OnUIFlowChanged;
		}
	}
}
