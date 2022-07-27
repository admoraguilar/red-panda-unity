using UnityEngine;

namespace FlappyBird
{
	public class Bird : MonoBehaviour
	{
		public float forceMultiplier = 5f;

		private Rigidbody2D _rigidbody2D = null;

		private void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			bool jumpInput = Input.GetKeyDown(KeyCode.F);
			if(jumpInput) {
				_rigidbody2D.velocity = Vector2.zero;
				_rigidbody2D.AddForce(Vector2.up * forceMultiplier, ForceMode2D.Impulse);
			}
		}
	}
}
