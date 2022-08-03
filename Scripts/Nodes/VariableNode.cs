using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
	public abstract class VariableNode : Node
	{
		[SerializeField, HideInInspector] protected NodePort output;

		public override void Initialize()
		{
			base.Initialize();
		}

		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State Execute()
		{
			return State.Success;
		}
	}
}