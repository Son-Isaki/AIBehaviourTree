using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Node
{
    public class RootNode : Node
	{
		public Node child;

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			if (child == null)
				return State.Success;
			return child.Update();
		}

		public override Node Clone()
		{
			RootNode node = Instantiate(this);
			if (child != null)
			{
				node.child = child.Clone();
			}
			return node;
		}
	}
}