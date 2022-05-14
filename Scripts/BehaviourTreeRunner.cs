using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	public class BehaviourTreeRunner : MonoBehaviour
	{
		[field: SerializeField] public BehaviourTree Tree { get; private set; }

		private void Start()
		{
			if (Tree == null)
				return;

			Tree = Tree.Clone();
			Tree.SetAttachedObject(gameObject);
			Tree.Bind();

			Tree.Execute(this, typeof(StartNode));
			Tree.Execute(this, typeof(UpdateNode));
		}
	}
}