using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Logic")]
	public class SequencerNode : CompositeNode
	{
		int current;

		protected override void OnStart()
		{
			current = 0;
		}

		protected override void OnStop()
		{

		}

		protected override State Execute()
		{
			var child = Children[current];

			switch (child.Update())
			{
				case State.Running:
					return State.Running;
					break;
				case State.Failure:
					return State.Failure;
					break;
				case State.Success:
					current++;
					break;
			}

			return current == Children.Count ? State.Success : State.Running;
		}
	}
}