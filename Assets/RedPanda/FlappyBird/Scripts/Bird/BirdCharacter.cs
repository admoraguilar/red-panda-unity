using UnityEngine;

namespace MicroFlappyBird
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class BirdCharacter : MonoBehaviour
	{
		public float jumpForceMultipier = 5f;

		private Rigidbody2D _rigidbody2D = null;

		public void Jump()
		{
			_rigidbody2D.velocity = Vector2.zero;
			_rigidbody2D.AddForce(Vector2.up * jumpForceMultipier, ForceMode2D.Impulse);
		}

		private void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}
	}
}
