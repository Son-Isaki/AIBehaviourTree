using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	[Category("World")]
	public class DistanceNode : VariableNode
	{
		[SerializeField, HideInInspector] NodePort positionA, positionB;

		public override void Initialize()
		{
			base.Initialize();
			positionA = AddInput("a", null, typeof(Vector3));
			positionB = AddInput("b", null, typeof(Vector3));
			output = AddValueOutput(typeof(float));
			SetDescription("Get the distance between two Vector3");
		}

		public override object GetValue()
		{
			return Vector3.Distance(GetInputValue<Vector3>(positionA), GetInputValue<Vector3>(positionB));
		}
	}
}