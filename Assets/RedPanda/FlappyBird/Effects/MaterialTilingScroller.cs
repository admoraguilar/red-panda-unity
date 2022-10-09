using UnityEngine;

namespace FlappyBird
{
	public class MaterialTilingScroller : MonoBehaviour
	{
		public bool shouldScroll = true;
		public Vector2 direction = Vector2.zero;
		public float speed = 2f;

		private Renderer _renderer = null;

		private void Awake()
		{
			_renderer = GetComponent<Renderer>();
		}

		private void FixedUpdate()
		{
			if(shouldScroll) {
				_renderer.material.mainTextureOffset += direction * speed * Time.deltaTime;
			}
		}
	}
}
