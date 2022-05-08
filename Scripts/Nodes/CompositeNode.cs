using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class CompositeNode : Node
    {
		public List<Node> children = new List<Node>();

		public override Node Clone()
		{
			CompositeNode node = Instantiate(this);
			node.children = children.ConvertAll(c => c.Clone());
			return node;
		}
	}
}