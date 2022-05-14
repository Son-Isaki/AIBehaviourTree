using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	public class RootNode : Node
	{
		[SerializeField, HideInInspector] protected NodePort output;

		public override void Initialize()
		{
			base.Initialize();
			output = AddOutput(N_OUTPUT, "");
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State Execute()
		{
			var nodes = GetLinkedNodes(output);

			foreach (var node in nodes)
			{
				if (node.Update() == State.Failure)
					return State.Failure;
			}

			return State.Success;
		}
	}
}