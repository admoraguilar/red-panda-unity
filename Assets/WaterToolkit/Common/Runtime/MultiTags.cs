using System.Collections.Generic;
using UnityEngine;

namespace WaterToolkit
{
	public class MultiTags : MonoBehaviour
	{
		[SerializeField]
		private List<string> _tags = new List<string>();

		public bool Contains(string tag) => _tags.Contains(tag);
	}
}
