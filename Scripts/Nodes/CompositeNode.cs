using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class CompositeNode : Node
    {
		public override void Initialize()
		{
			base.Initialize();
			AddInput(N_INPUT, "");
			AddOutput(N_OUTPUT, "", null, UnityEditor.Experimental.GraphView.Port.Capacity.Multi);
		}
	}
}