using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace WaterToolkit.Sequences
{
	public class Fader : MonoBehaviour
	{
		public event Action<float> OnFade = delegate { };

		[SerializeField]
		private float speed = 5f;

		[SerializeField]
		private Image _image = null;

		public IEnumerator FadeTo(float from, bool toClear, Action<float> onFade = null)
		{
			_image.color = SetA(_image.color, Mathf.Clamp(from, 0f, 1f));

			float target = toClear ? 0f : 1f;
			float lerp = 0f;

			while(_image.color.a != target) {
				_image.color = Color.Lerp(_image.color, SetA(_image.color, target), lerp);
				lerp += Time.deltaTime * speed;
				onFade?.Invoke(lerp);
				OnFade(lerp);
				yield return null;
			}
		}

		private Color SetA(Color value, float a)
		{
			return new Color(
				_image.color.r, _image.color.g,
				_image.color.b, a);
		}

		private void Start()
		{
			_image.color = SetA(_image.color, 0f);
		}
	}
}
