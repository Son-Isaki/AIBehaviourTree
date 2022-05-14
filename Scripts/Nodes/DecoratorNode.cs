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
			AddInput(N_INPUT, "");
			AddOutput(N_OUTPUT, "");
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}
	}
}