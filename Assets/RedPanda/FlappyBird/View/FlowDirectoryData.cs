using UnityEngine;
using WaterToolkit.FlowTrees;

namespace FlappyBird
{
	public class FlowDirectoryData : MonoBehaviour
	{
		[SerializeField]
		private Node _menu = null;

		[SerializeField]
		private Node _game = null;

		[SerializeField]
		private Node _gameOver = null;

		public Node menu => _menu;

		public Node game => _game;

		public Node gameOver => _gameOver;
	}
}
