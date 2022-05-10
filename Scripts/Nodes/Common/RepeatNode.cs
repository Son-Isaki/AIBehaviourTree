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

		protected override void OnStop()
		{

		}

		protected override State OnUpdate()
		{
			for (int i = 0; i < Children.Count; i++) 
				Children[i].Update();

			count++;
			if (count >= loopCount)
			{
				return State.Success;
			}
			return State.Running;
		}

		public override string GetDescription()
		{
			if (loopCount <= 0)
			{
				return $"Repeat indefinitely";
			}
			else
			{
				return $"Repeat {loopCount} times";
			}
		}
	}
}