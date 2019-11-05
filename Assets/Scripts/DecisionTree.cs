using System;

namespace AI.DecisionTree
{
    abstract class Node
    {
        public virtual void Eval() { }
    }
    
    class BinaryDecisionNode : Node
    {
        private Node trueChild, falseChild;
        Func<bool> condition;

        public BinaryDecisionNode(Node trueChild, Node falseChild, Func<bool> condition)
        {
            this.trueChild = trueChild;
            this.falseChild = falseChild;
            this.condition = condition;
        }

        public override void Eval()
        {

            if (condition())
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
}
