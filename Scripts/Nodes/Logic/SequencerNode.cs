using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Logic")]
	public class SequencerNode : CompositeNode
	{
		protected override State Execute()
		{
			var nodes = GetOutputNodes(output);

			foreach (var node in nodes)
			{
				if (node.Update() == State.Failure)
				{
					return State.Failure;
				}
			}

			return State.Success;
		}
	}
}