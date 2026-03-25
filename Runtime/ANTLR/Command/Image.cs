using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Image : Command
    {
        private StartExpr expression;

        public Image(StartExpr expression)
        {
            this.expression = expression;
        }

        public override string ToString()
        {
            // Debug.Log("print ToString : " + expression);
            return "(image: " + expression.ToString() + ")";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            Sprite sprite = story.GetSprite(expression.Evaluate(story, storyNode));
            storyNode.SetSprite(sprite);
            return "";
        }
    }
}
