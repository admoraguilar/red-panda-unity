using UnityEngine;

namespace FlappyBird
{
	public class BirdController : MonoBehaviour
	{
		private BirdCharacter _character = null;

		private void Awake()
		{
			_character = GetComponent<BirdCharacter>();
		}

		private void Update()
		{
			bool jumpInput = Input.GetKeyDown(KeyCode.F);
			if(jumpInput) { _character.Jump(); }
		}
	}
}
