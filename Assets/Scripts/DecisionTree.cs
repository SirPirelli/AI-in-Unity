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
        Condition condition;

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

    class ArgAction<T> : Node
    {
        Action<T> action;
        T parameter;

        public ArgAction(Action<T> action, T parameter)
        {
            this.action = action;
            this.parameter = parameter;
        }

        public override void Eval()
        {
            action(parameter);
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
