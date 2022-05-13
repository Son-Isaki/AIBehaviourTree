using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Event")]
	public class StartNode : RootNode
	{
		public override string GetDescription()
		{
			return "On Start event";
		}
	}
}