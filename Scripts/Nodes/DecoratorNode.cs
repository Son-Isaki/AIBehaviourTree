using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class DecoratorNode : Node
    {
		public override void Initialize()
		{
			base.Initialize();
			AddInput(N_INPUT, "", GetNodePortType());
			AddOutput(N_OUTPUT, "", GetNodePortType());
		}
	}
}