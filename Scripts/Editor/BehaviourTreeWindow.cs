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

        private BehaviourTree currentTree;
        private BehaviourTreeView treeView;
        private Label treeTitleLabel;

        [MenuItem("Window/Isaki/AI Behaviour Tree")]
        public static void OpenWindow()
        {
            CurrentWindow = GetWindow<BehaviourTreeWindow>();
            CurrentWindow.titleContent = new GUIContent("AI Behaviour Tree");
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

		private void OnEnable()
		{
            Initialize();
            Selection.selectionChanged += Initialize;
			EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

		private void OnDisable()
        {
            rootVisualElement.Clear();
            Selection.selectionChanged -= Initialize;
            EditorApplication.playModeStateChanged -= PlayModeStateChanged;
        }

        private void Initialize()
        {
            rootVisualElement.Clear();

            var box = new Box()
            {
                style = {
                    alignItems = Align.Center,
                    justifyContent = Justify.Center,
                }
            };
            box.StretchToParentSize();

            if (Selection.activeObject == null || Selection.activeObject is not BehaviourTree)
            {
                var label = new Label($"Please select a Behaviour Tree");
                box.Add(label);
                rootVisualElement.Add(box);
                return;
            }

            currentTree = Selection.activeObject as BehaviourTree;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UIBUILDER_PATH + "BehaviourTreeEditor.uxml");
            visualTree.CloneTree(rootVisualElement);

            treeTitleLabel = rootVisualElement.Q<Label>("tree-title");
            treeTitleLabel.text = currentTree.name;
            treeView = rootVisualElement.Q<BehaviourTreeView>();

            if (treeView != null)
            {
                treeView.PopulateView(currentTree);
                treeView.OnNodeSelected = OnNodeSelectionChanged;
            }
        }

        private void PlayModeStateChanged(PlayModeStateChange mode)
        {
        }

        void OnNodeSelectionChanged(NodeView nodeView)
		{
		}
	}
}