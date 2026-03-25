using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Sound : Command
    {
        private StartExpr expression;

        public Sound(StartExpr expression)
        {
            this.expression = expression;
        }

        public override string ToString()
        {
            // Debug.Log("print ToString : " + expression);
            return "(sound: " + expression.ToString() + ")";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            AudioClip sound = story.GetSound(expression.Evaluate(story, storyNode));
            storyNode.SetSound(sound);
            return "";
        }
    }
}
