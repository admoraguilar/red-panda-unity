using UnityEngine;

namespace WaterToolkit
{
	[ExecuteAlways]
	[RequireComponent(typeof(Camera))]
	public class CameraOrthographicExpander : MonoBehaviour
	{
		public enum Mode
		{
			None,
			Horizontal,
			Vertical,
			Fixed
		}

		[SerializeField]
		private Mode _mode = Mode.None;

		[SerializeField]
		private float _baseOrthographicSize = 5f;

		[SerializeField]
		private Vector2 _referenceAspectRatio = new Vector2(21, 9);

		private Camera _camera = null;

		private void UpdateCameraExtents()
		{
			if(!_camera.orthographic) {
				Debug.LogWarning("Camera Ortographic Expand Resolver only works if the camera projection is set to 'Ortographic'.");
				return; 
			}

			float curScreenValue = (float)Screen.width / (float)Screen.height;
			float preferredScreenValue = _referenceAspectRatio.x / _referenceAspectRatio.y;
			float preferredHorizontalExtent = _baseOrthographicSize * preferredScreenValue;
			float preferredVerticalExtent = preferredHorizontalExtent / curScreenValue;

			if(_mode == Mode.None) {
				_camera.orthographicSize = _baseOrthographicSize;
			} else if(_mode == Mode.Horizontal) {
				if(_baseOrthographicSize < preferredVerticalExtent) {
					_camera.orthographicSize = preferredHorizontalExtent;
				}
			} else if(_mode == Mode.Vertical) {
				if(_baseOrthographicSize > preferredVerticalExtent) {
					_camera.orthographicSize = preferredVerticalExtent;
				}
			} else if(_mode == Mode.Fixed) {
				_camera.orthographicSize = preferredVerticalExtent;
			}
		}

		private void Awake()
		{
			_camera = GetComponent<Camera>();
		}

		private void Update()
		{
			UpdateCameraExtents();
		}

#if UNITY_EDITOR
		
		private void Reset()
		{
			_baseOrthographicSize = _camera.orthographicSize;
		}

#endif
	}
}
