using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	public class BehaviourTreeRunner : MonoBehaviour
	{
		[field: SerializeField] public BehaviourTree Tree { get; private set; }
		[field: SerializeField] public Blackboard Blackboard{ get; private set; }

		private void Start()
		{
			if (Tree == null)
				return;

			Tree = Tree.Clone();
			Tree.SetBlackboard(Blackboard.Clone());
			Tree.Bind();
		}

		private void Update()
		{
			if (Tree == null)
				return;

			Tree.Update();
		}
	}
}