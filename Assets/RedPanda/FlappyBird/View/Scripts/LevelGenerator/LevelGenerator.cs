using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBird
{
	public class LevelGenerator : MonoBehaviour
	{
		public List<Transform> obstaclePrefabList = new List<Transform>();
		public float obstacleDistance = 5f;
		public float maxObstacleGeneration = 5f;
		public float obstacleMoveSpeed = 5f;

		private List<Transform> _obstacleInstanceList = new List<Transform>();

		private Transform _transform = null;

#if UNITY_EDITOR

		private bool _isEditorDirty = false;

#endif

		private IEnumerator GenerationRoutine()
		{
			while(true) {
				bool isDirty = false;

#if UNITY_EDITOR

				isDirty = _isEditorDirty;
				_isEditorDirty = false;

#endif

				if(!isDirty) {
					if(_obstacleInstanceList.Count >= maxObstacleGeneration) {
						yield return null;
					}
				} else {
					isDirty = false;
				}

				// Generate more obstacles
				while(_obstacleInstanceList.Count < maxObstacleGeneration) {
					Transform obstaclePrefab = GetObstaclePrefab();
					Transform obstacleInstance = Instantiate(obstaclePrefab, _transform);
					obstacleInstance.localPosition = Vector3.zero;
					_obstacleInstanceList.Add(obstacleInstance);
				}

				// Update positioning
				Transform pivot = _obstacleInstanceList[0];
				for(int i = 1; i < _obstacleInstanceList.Count; i++) {
					Transform obstacleInstance = _obstacleInstanceList[i];
					obstacleInstance.position = new Vector3(
						pivot.position.x + obstacleDistance,
						pivot.position.y,
						pivot.position.z);
					pivot = obstacleInstance;
				}

				for(int i = 0; i < _obstacleInstanceList.Count;) {
					Transform obstacleInstance = _obstacleInstanceList[i];
					if(obstacleInstance.position.x <= -20f) {
						Destroy(obstacleInstance.gameObject);
						_obstacleInstanceList.Remove(obstacleInstance);
					} else {
						i++;
					}
				}

				yield return null;
			}
		}

		private Transform GetObstaclePrefab()
		{
			int index = Random.Range(0, obstaclePrefabList.Count);
			Transform result = obstaclePrefabList[index];
			return result;
		}

		private void Awake()
		{
			_transform = GetComponent<Transform>();
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.R)) {
				StartCoroutine(GenerationRoutine());
			}
		}

		private void FixedUpdate()
		{
			foreach(Transform obstacleInstance in _obstacleInstanceList) {
				obstacleInstance.position += Vector3.left * obstacleMoveSpeed * Time.deltaTime;
			}
		}

#if UNITY_EDITOR

		private void OnValidate()
		{
			_isEditorDirty = true;
		}

#endif
	}
}
