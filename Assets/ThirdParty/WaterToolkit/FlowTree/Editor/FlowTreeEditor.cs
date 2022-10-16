using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using UEditor = UnityEditor.Editor;

namespace WaterToolkit.FlowTrees.Editor
{
	[CustomEditor(typeof(FlowTree), true)]
	public class FlowTreeEditor : UEditor
	{
		private List<string> _excludePropertiesList = new List<string>();

		private SerializedProperty _script = null;
		private SerializedProperty _nodeStack = null;

		public FlowTree flowTree
		{
			get {
				if(_flowTree == null) {
					_flowTree = target as FlowTree;
				}
				return _flowTree;
			}
		}
		private FlowTree _flowTree = null;

		private void OnEnable()
		{
			_script = serializedObject.FindProperty("m_Script");
			_nodeStack = serializedObject.FindProperty("_nodeStack");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if(Application.isPlaying && targets.Length == 1) {
				EditorGUILayout.Space();

				if(GUILayout.Button("Go to Current Node")) { Selection.activeObject = flowTree.current; }

				using(new EditorGUILayout.HorizontalScope()) {
					if(GUILayout.Button("Next")) { flowTree.Next(); }
					if(GUILayout.Button("Next Next Frame")) { flowTree.NextNow(); }
				}
				
				using(new EditorGUILayout.HorizontalScope()) {
					if(GUILayout.Button("Pop")) { flowTree.Pop(); }
					if(GUILayout.Button("Pop Next Frame")) { flowTree.PopNow(); }
				}

				EditorGUILayout.Space();
			}

			_excludePropertiesList.Clear();
			_excludePropertiesList.Add("m_Script");
			_excludePropertiesList.Add("_nodeStack");

			using(new EditorGUI.DisabledGroupScope(true)) {
				EditorGUILayout.PropertyField(_script);
			}

			DrawPropertiesExcluding(serializedObject, _excludePropertiesList.ToArray());

			using(new EditorGUI.DisabledGroupScope(true)) {
				EditorGUILayout.PropertyField(_nodeStack, true);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}