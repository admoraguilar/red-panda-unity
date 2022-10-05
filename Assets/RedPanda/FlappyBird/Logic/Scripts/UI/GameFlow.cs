using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
	[DefaultExecutionOrder(-1)]
	public class GameFlow : MonoBehaviour
	{
		private FlowData flowData = null;

		private void OnUIFlowChanged()
		{
			Debug.Log($"Response from {GetType()}: UI Flow changed to - [{flowData.uiFlow}].");
		}

		private void Awake()
		{
			FlappyData.instance.Deserialize();
			flowData = FlappyData.instance.Get<FlowData>();
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.Q)) {
				FlappyData.instance.Serialize();
				Debug.Log("Serialize");
			}

			if(Input.GetKeyDown(KeyCode.W)) {
				flowData.uiFlow = $"StartMenu.{Time.time:00.00}";
			}

			if(Input.GetKeyDown(KeyCode.E)) {
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
