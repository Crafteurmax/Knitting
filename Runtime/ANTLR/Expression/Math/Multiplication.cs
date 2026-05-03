using System;
using Runtime.ANTLR.TwineValue;
using ValueType = Runtime.ANTLR.Value.ValueType;

namespace Runtime.ANTLR.Expression
{
    public class Multiplication : Expression
    {
        
        private Expression leftExpression;
        private Expression rightExpression;

        public Multiplication(Expression leftExpression, Expression rightExpression)
        {
            this.leftExpression = leftExpression;
            this.rightExpression = rightExpression;
        }

        override public Value.Value Evaluate(Story story, StoryNode storyNode)
        {
            Value.Value left = leftExpression.Evaluate(story, storyNode);
            Value.Value right = rightExpression.Evaluate(story, storyNode);

            // NON TRIVIAL ELEMENTS
            if(left.valueType == ValueType.String || right.valueType == ValueType.String) throw new NotImplementedException(); //TODO
            if(left.valueType == ValueType.Array || right.valueType == ValueType.Array) throw new NotImplementedException(); //TODO
            if(left.valueType == ValueType.Boolean && right.valueType == ValueType.Boolean) throw new NotImplementedException(); //TODO
            
            // Trivial elements
            if(left.valueType == ValueType.Integer && right.valueType == ValueType.Integer) return new Value.IntValue((int) left * (int) right);
            
            if(left.valueType ==  ValueType.Floating || right.valueType == ValueType.Floating) return new Value.FloatValue((float) left * (float) right);
            
            if(left.valueType == ValueType.Boolean && right.valueType == ValueType.Integer) 
            {
                return new Value.IntValue(left *  (int)right);
            }
            
            if(right.valueType == ValueType.Boolean && left.valueType == ValueType.Integer)
            {
                return new Value.IntValue((int)left *  right);
            }
            
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return leftExpression.ToString() + " * " + rightExpression.ToString();
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            throw new NotImplementedException(); // TODO
        }
    }
}
