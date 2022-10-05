using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
	public class FlowData
	{
		public event Action OnUIFlowChanged = delegate { };

		private string _uiFlow = string.Empty;

		public string uiFlow
		{
			get => _uiFlow;
			set {
				_uiFlow = value;
				OnUIFlowChanged();
			}
		}
	}
}
