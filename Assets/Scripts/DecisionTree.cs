using System;
using UnityEngine;

namespace AI.DecisionTree
{
    abstract class Node
    {
        public virtual void Eval() { }
    }
    
    class BinaryDecisionNode : Node
    {
        private Node trueChild, falseChild;
        /*Func<bool>*/Condition condition;

        public BinaryDecisionNode(Node trueChild, Node falseChild, Condition condition)
        {
            this.trueChild = trueChild;
            this.falseChild = falseChild;
            this.condition = condition;
        }

        public override void Eval()
        {

            if (condition.CheckCondition())
                trueChild.Eval();
            else
                falseChild.Eval();
        }
    }

    class ActionNode : Node
    {
        Action action;

        public ActionNode(Action action)
        {
            this.action = action;
        }

        public override void Eval()
        {
            action();
        }
    }

    class Condition
    {
        public virtual bool CheckCondition() { return false; }
    }

    class ArgCondition<T> : Condition
    {

        public Func<T, bool> condition;
        public T arg;

        public ArgCondition(Func<T, bool> condition, T arg)
        {
            this.condition = condition;
            this.arg = arg;
        }

        public override bool CheckCondition()
        {
            return condition.Invoke(arg);
        }
    }
}
