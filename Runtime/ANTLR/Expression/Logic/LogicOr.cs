using System;
using Runtime.ANTLR.TwineValue;
using ValueType = Runtime.ANTLR.Value.ValueType;

namespace Runtime.ANTLR.Expression
{
    public class LogicOr : Expression
    {
        
        private Expression leftExpression;
        private Expression rightExpression;

        public LogicOr(Expression leftExpression, Expression rightExpression)
        {
            this.leftExpression = leftExpression;
            this.rightExpression = rightExpression;
        }

        override public Value.Value Evaluate(Story story, StoryNode storyNode)
        {
            Value.Value left = leftExpression.Evaluate(story, storyNode);
            Value.Value right = rightExpression.Evaluate(story, storyNode);

            // NON TRIVIAL ELEMENTS
            if(left.valueType != ValueType.Boolean || right.valueType != ValueType.Boolean) throw new NotImplementedException(); //TODO
            
            // Trivial elements
            
            if(left.valueType == ValueType.Boolean && right.valueType == ValueType.Boolean) return new Value.Value((bool)left || (bool) right);
            
            
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return leftExpression.ToString() + " or " + rightExpression.ToString();
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            throw new NotImplementedException(); // TODO
        }
    }
}
