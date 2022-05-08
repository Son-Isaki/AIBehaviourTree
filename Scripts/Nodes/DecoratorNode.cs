using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class DecoratorNode : Node
    {
		[SerializeField, HideInInspector] public Node child;

		public override Node Clone()
		{
			DecoratorNode node = Instantiate(this);
			node.child = child.Clone();
			return node;
		}
	}
}