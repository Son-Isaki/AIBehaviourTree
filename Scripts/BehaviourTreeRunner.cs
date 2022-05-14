using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	public class BehaviourTreeRunner : MonoBehaviour
	{
		[field: SerializeField] public BehaviourTree Tree { get; private set; }
		[SerializeField, Range(.001f, .25f)] private float tickInterval = .05f;

		private void Start()
		{
			if (Tree == null)
				return;

			Tree = Tree.Clone();
			Tree.SetAttachedObject(gameObject);
			Tree.Bind();

			Tree.Execute(this, typeof(StartNode), tickInterval);
			Tree.Execute(this, typeof(UpdateNode), tickInterval);
		}
	}
}