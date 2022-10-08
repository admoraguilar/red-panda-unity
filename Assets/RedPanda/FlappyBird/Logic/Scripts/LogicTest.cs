using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaterToolkit.Data;
using WaterToolkit.FlowTrees;

public class LogicTest : MonoBehaviour
{
	private void Awake()
	{
		IEnumerable<Link> links = Blackboard.GetMany<Link>(l => l.shouldHandleGameObjectActive == false);
		foreach(Link link in links) {
			Debug.Log(link.name);
		}
	}
}
