using System;
using Runtime.ANTLR.TwineValue;
using ValueType = Runtime.ANTLR.Value.ValueType;

namespace Runtime.ANTLR.Expression
{
    public class Parentheses : Expression
    {
        Expression expression;
        // Start is called before the first frame update
        public Parentheses(Expression expression)
        {
            this.expression = expression;
        }

        override public Value.Value Evaluate(Story story, StoryNode storyNode)
        {
            return expression.Evaluate(story, storyNode);
        }

        public override string ToString()
        {
            return  "(" + expression.ToString() + ")";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            throw new NotImplementedException(); // TODO
        }
    }
}
