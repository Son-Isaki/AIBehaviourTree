using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace AIBehaviourTree.Node
{
    public class BehaviourTreeWindow : EditorWindow
    {
        public const string BASE_PATH = "Assets/Packages/UnityAIBehaviourTree/";
        public const string UIBUILDER_PATH = BASE_PATH + "UIBuilder/";

        public static Vector2 CurrentPosition {
			get {
                return GetWindow<BehaviourTreeWindow>().position.position;
			}
		}

        public static BehaviourTreeWindow CurrentWindow { get; private set; }

        BehaviourTree currentTree;
        Label treeTitleLabel;
        BehaviourTreeView treeView;
        InspectorView inspectorView;
        IMGUIContainer blackboardView;
        SerializedObject treeObject;
        SerializedProperty blackboardProperty;

        [MenuItem("Window/Isaki/AI Behaviour Tree")]
        public static void OpenWindow()
        {
            CurrentWindow = GetWindow<BehaviourTreeWindow>();
            CurrentWindow.titleContent = new GUIContent("AI Behaviour Tree");
            //CurrentWindow.position = new Rect(0, 0, 800, 600);
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is BehaviourTree)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UIBUILDER_PATH + "BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(UIBUILDER_PATH + "BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeTitleLabel = root.Q<Label>("tree-title");
            treeView = root.Q<BehaviourTreeView>();
            inspectorView = root.Q<InspectorView>();
            blackboardView = root.Q<IMGUIContainer>();
            blackboardView.onGUIHandler = () => {
                if (treeObject != null && treeObject.targetObject != null)
                {
                    treeObject.Update();
                    EditorGUILayout.PropertyField(blackboardProperty);
                    treeObject.ApplyModifiedProperties();
                }
            };

            treeView.OnNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();
        }

		private void OnEnable()
		{
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

		private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

		private void OnPlayModeStateChanged(PlayModeStateChange playModeState)
		{
			switch (playModeState)
			{
				case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
					break;
				case PlayModeStateChange.ExitingEditMode:
					break;
				case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
				case PlayModeStateChange.ExitingPlayMode:
					break;
			}
		}

        private void OnSelectionChange()
        {
            if (Selection.activeObject is BehaviourTree tree)
            {
                currentTree = tree;
            }
            else
            {
                if (Selection.activeGameObject)
                {
                    BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                    if (runner)
                    {
                        currentTree = runner.Tree;
                    }
                }
            }

            if (currentTree != null)
            {
                treeObject = new SerializedObject(currentTree);
                blackboardProperty = treeObject.FindProperty("blackboard");
                if (treeTitleLabel != null)
                {
                    treeTitleLabel.text = currentTree.name;
                }
                treeView?.PopulateView(currentTree);
            }
        }

        void OnNodeSelectionChanged(NodeView nodeView)
		{
            inspectorView.UpdateSelection(nodeView);
		}

		private void OnInspectorUpdate()
		{
            treeView?.UpdateNodeStates();
		}
	}
}