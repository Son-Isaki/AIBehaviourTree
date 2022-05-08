using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Node
{
    public abstract class VariableNode : Node
    {
		public Node child;

		public override Node Clone()
		{
			VariableNode node = Instantiate(this);
			node.child = child.Clone();
			return node;
		}
	}
}