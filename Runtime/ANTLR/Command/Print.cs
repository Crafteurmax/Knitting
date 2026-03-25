using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Print : Command
    {
        private StartExpr expression;

        public Print(StartExpr expression)
        {
            this.expression = expression;
        }

        public override string ToString()
        {
            // Debug.Log("print ToString : " + expression);
            return "(print: " + expression.ToString() + ")";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            // Debug.Log("collapse print");
            return expression.Collapse(story, storyNode);
        }
    }
}
