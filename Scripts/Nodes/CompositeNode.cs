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
			AddInput(DEFAULT_INPUT_NAME, "");
			AddOutput(DEFAULT_OUTPUT_NAME, "", typeof(float), UnityEditor.Experimental.GraphView.Port.Capacity.Multi);
		}
	}
}