using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class CompositeNode : Node
	{
		[SerializeField, HideInInspector] protected NodePort input, output;

		public override void Initialize()
		{
			base.Initialize();
			input = AddInput(N_INPUT, "");
			output = AddOutput(N_OUTPUT, "", null, UnityEditor.Experimental.GraphView.Port.Capacity.Multi);
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{

		}
	}
}