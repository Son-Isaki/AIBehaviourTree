using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	[Category("Event")]
	public class UpdateNode : RootNode
	{
		public override string GetDescription()
		{
			return "On Update event";
		}
	}
}