using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviourTree.Node
{
    public abstract class ActionNode : Node
    {
        [SerializeField, HideInInspector] protected NodePort input;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            input = AddInput(N_INPUT, "");
        }
    }
}