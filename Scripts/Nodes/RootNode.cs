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
			AddOutput(DEFAULT_OUTPUT_NAME, "");
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
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

		public override Node Clone()
		{
			RootNode node = Instantiate(this);
			node.Children = Children.ConvertAll(c => c.Clone());
			return node;
		}
	}
}