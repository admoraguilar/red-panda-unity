using System;
using UnityEngine;

namespace FlappyBird
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class BirdCharacter : MonoBehaviour
	{
		public event Action<Collision2D> OnCollisionEnter2DCallback = delegate { };
		public event Action<Collider2D> OnTriggerEnter2DCallback = delegate { };
		public event Action OnUpdate = delegate { };
		public event Action OnJump = delegate { };

		public float jumpForceMultipier = 5f;
		public float rotationSpeed = 1f;

		private Transform _transform = null;
		private Rigidbody2D _rigidbody2D = null;

		public new Transform transform => _transform;
		public new Rigidbody2D rigidbody2D => _rigidbody2D;

		public void Jump()
		{
			_rigidbody2D.velocity = Vector2.zero;
			_rigidbody2D.AddForce(Vector2.up * jumpForceMultipier, ForceMode2D.Impulse);

			_transform.rotation = Quaternion.Euler(new Vector3(
				_transform.rotation.x,
				_transform.rotation.y,
				35f));

			OnJump();
		}

		private void Awake()
		{
			_transform = GetComponent<Transform>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			OnUpdate();
		}

		private void FixedUpdate()
		{
			Quaternion rotateTarget = Quaternion.Euler(0f, 0f, -90f);
			_transform.rotation = Quaternion.RotateTowards(
				_transform.rotation, rotateTarget,
				rotationSpeed * Mathf.Abs(Mathf.Clamp(_rigidbody2D.velocity.y, -10f, 0f)) * Time.deltaTime);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			OnCollisionEnter2DCallback(collision);
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			OnTriggerEnter2DCallback(collider);
		}
	}
}
