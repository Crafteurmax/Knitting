using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Lowercase : Command
    {
        private StartExpr expression;

        public Lowercase(StartExpr expression)
        {
            this.expression = expression;
        }

        public override string ToString()
        {
            return "(lowercase: " + expression.ToString() + ")";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            return expression.Collapse(story, storyNode).ToLower();
        }
    }
}
