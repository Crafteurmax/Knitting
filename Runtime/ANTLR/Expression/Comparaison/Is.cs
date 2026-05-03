using System;
using Runtime.ANTLR.TwineValue;
using ValueType = Runtime.ANTLR.Value.ValueType;

namespace Runtime.ANTLR.Expression
{
    public class Is : Expression
    {
        
        private Expression leftExpression;
        private Expression rightExpression;

        public Is(Expression leftExpression, Expression rightExpression)
        {
            this.leftExpression = leftExpression;
            this.rightExpression = rightExpression;
        }

        override public Value.Value Evaluate(Story story, StoryNode storyNode)
        {
            Value.Value left = leftExpression.Evaluate(story, storyNode);
            Value.Value right = rightExpression.Evaluate(story, storyNode);

            return new Value.BoolValue(left.CompareTo(right) == 0);
        }

        public override string ToString()
        {
            return leftExpression.ToString() + " is " + rightExpression.ToString();
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            throw new NotImplementedException(); // TODO
        }
    }
}
