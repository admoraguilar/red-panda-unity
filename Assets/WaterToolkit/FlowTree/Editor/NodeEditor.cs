using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using UEditor = UnityEditor.Editor;

namespace WaterToolkit.FlowTrees.Editor
{	
	[CanEditMultipleObjects]
	[CustomEditor(typeof(Node), true)]
	public class NodeEditor : UEditor
	{
		private List<string> _excludePropertiesList = new List<string>();

		private SerializedProperty _script = null;

		public Node node
		{
			get {
				if(_node == null) {
					_node = target as Node;
				}
				return _node;
			}
		}
		private Node _node = null;

		private void OnEnable()
		{
			_script = serializedObject.FindProperty("m_Script");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if(targets.Length == 1) {
				if(node.internal_description != string.Empty) {
					EditorGUILayout.HelpBox(node.internal_description, MessageType.Info, true);
				}

				if(Application.isPlaying && targets.Length == 1) {
					EditorGUILayout.Space();

					using(new EditorGUILayout.VerticalScope()) {
						if(GUILayout.Button("Go to Tree")) { Selection.activeObject = (Component)node.tree; }

						using(new EditorGUILayout.HorizontalScope()) {
							if(GUILayout.Button("Next")) { node.Next(); }
							if(GUILayout.Button("Next Now")) { node.NextNow(); }
						}

						using(new EditorGUILayout.HorizontalScope()) {
							if(GUILayout.Button("Set")) { node.Set(); }
							if(GUILayout.Button("Set Now")) { node.SetNow(); }
						}

						using(new EditorGUILayout.HorizontalScope()) {
							if(GUILayout.Button("Push")) { node.Push(); }
							if(GUILayout.Button("Push Now")) { node.PushNow(); }
						}

						using(new EditorGUILayout.HorizontalScope()) {
							if(GUILayout.Button("Pop Until Removed")) { node.PopUntilRemoved(); }
							if(GUILayout.Button("Pop Until Removed Now")) { node.PopUntilRemovedNow(); }
						}

						using(new EditorGUILayout.HorizontalScope()) {
							if(GUILayout.Button("Swap")) { node.Swap(); }
							if(GUILayout.Button("Swap Now")) { node.SwapNow(); }
						}
					}

					EditorGUILayout.Space();
				}
			}

			bool shouldDrawFlowTreeField = true;
			foreach(Node target in targets) {
				if(target.GetComponentInParent<FlowTree>() != null) {
					shouldDrawFlowTreeField = false;
					break;
				}
			}

			using(new EditorGUI.DisabledGroupScope(true)) {
				EditorGUILayout.PropertyField(_script);
			}

			_excludePropertiesList.Clear();
			_excludePropertiesList.Add("m_Script");
			if(!shouldDrawFlowTreeField) { 
				_excludePropertiesList.Add("_tree"); 
			}

			DrawPropertiesExcluding(serializedObject, _excludePropertiesList.ToArray());

			serializedObject.ApplyModifiedProperties();
		}
	}
}

