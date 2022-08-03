using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIBehaviourTree.Node
{
	[Category("World")]
	public class GetPositionNode : VariableNode
	{
		[SerializeField, HideInInspector] NodePort gameObject;

		public override void Initialize()
		{
			base.Initialize();
			gameObject = AddInput("gameObject", "GameObject", typeof(GameObject));
			output = AddValueOutput(typeof(Vector3));
			SetDescription("Get the world position of GameObject");
		}

		public override object GetValue()
		{
			return GetInputValue<GameObject>(gameObject).transform.position;
		}
	}
}