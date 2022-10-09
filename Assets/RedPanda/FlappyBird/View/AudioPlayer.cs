using UnityEngine;

namespace FlappyBird
{
	public class AudioPlayer : MonoBehaviour
	{
		public AudioClip pointSFX = null;
		public AudioClip dieSFX = null;
		public AudioClip hitSFX = null;
		public AudioClip swooshSFX = null;
		public AudioClip wingFlapSFX = null;

		[Space]
		[SerializeField]
		private AudioSource _audioSource = null;

		public void PlayPointSFX() => PlaySFX(pointSFX);
		public void PlayDieSFX() => PlaySFX(dieSFX);
		public void PlayHitSFX() => PlaySFX(hitSFX);
		public void PlaySwooshSFX() => PlaySFX(swooshSFX);
		public void PlayWingFlapSFX() => PlaySFX(wingFlapSFX);

		private void PlaySFX(AudioClip clip)
		{
			_audioSource.PlayOneShot(clip);
		}
	}
}

