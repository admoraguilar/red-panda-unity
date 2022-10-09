using UnityEngine;
using WaterToolkit.FlowTrees;

namespace FlappyBird
{
	public class FlowDirectoryData : MonoBehaviour
	{
		[SerializeField]
		private Node _initialize = null;

		[SerializeField]
		private Node _preGame = null;

		[SerializeField]
		private Node _inGame = null;

		[SerializeField]
		private Node _gameOver = null;

		public Node initialize => _initialize;

		public Node preGame => _preGame;

		public Node inGame => _inGame;

		public Node gameOver => _gameOver;
	}
}
