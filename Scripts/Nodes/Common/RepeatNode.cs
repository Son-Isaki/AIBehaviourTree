using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Node
{
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
			child.Update();
			count++;
			if (count >= loopCount)
			{
				return State.Success;
			}
			return State.Running;
		}
	}
}