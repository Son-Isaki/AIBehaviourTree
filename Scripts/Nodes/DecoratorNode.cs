using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class DecoratorNode : Node
    {
		[SerializeField, HideInInspector] protected NodePort input, output;

		public override void Initialize()
		{
			base.Initialize();
			input = AddInput(N_INPUT, "");
			output = AddOutput(N_OUTPUT, "");
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}
	}
}