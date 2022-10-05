using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
	public class FlowData
	{
		public int la = 1;

		private string _flow = string.Empty;

		private int _flowCount = 0;

		private float test = 0.2f;

		private float test3 = 0.5f;

		private double test4 = 1.4f;

		public string flow
		{
			get => _flow;
			set => _flow = value;
		}

		public int flowCount => _flowCount;

		public float testing2
		{
			get => test3;
			set => test3 = value;
		}
	}
}
