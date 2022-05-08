using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    [System.Serializable]
    public class BlackboardAI : Blackboard
    {
        public GameObject Target;
        public Vector3 TargetPosition;
    }
}