using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Node
{
	public class MoveToTargetLocationNode : ActionNode
	{
		protected override void OnStart()
		{
		}

		protected override void OnStop()
		{
		}

		protected override State OnUpdate()
		{
			/*if (Blackboard.self.TargetEnemy != null)
			{
				Blackboard.self.SetDestination(Blackboard.self.TargetEnemy.transform.position);
			}
			else
			{
				Blackboard.self.SetDestination(Blackboard.self.TargetPosition);
			}

			Blackboard.self.Move();

			if (Blackboard.self.Agent.remainingDistance <= Blackboard.self.Agent.stoppingDistance)
			{
				Blackboard.self.Stop();
				return State.Success;
			}*/

			return State.Running;
		}
	}
}