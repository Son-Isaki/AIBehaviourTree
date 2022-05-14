using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Logic")]
    public class RepeatNode : DecoratorNode
	{
		public int loopCount = 3;
		int count;

		protected override void OnStart()
		{
			count = 0;
		}

		protected override State Execute()
		{
			for (count = 0; count < loopCount; count++)
			{
				var node = GetLinkedNode(output);
				node.Update();
			}
			return State.Success;
		}

		public override string GetDescription()
		{
			if (loopCount > 1)
			{
				return $"Repeat {loopCount} times";
			}
			else
			{
				return $"Repeat {loopCount} time";
			}
		}
	}
}