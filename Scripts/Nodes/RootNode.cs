using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	public class RootNode : Node
	{
		public override void Initialize()
		{
			base.Initialize();
			AddOutput(N_OUTPUT, "", GetNodePortType());
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State Execute()
		{
			if (Children.Count == 0)
				return State.Success;

			for (int i = 0; i < Children.Count; i++)
			{
				if (Children[i].Update() == State.Failure)
					return State.Failure;
			}

			return State.Success;
		}
	}
}