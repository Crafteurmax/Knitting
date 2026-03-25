using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Execute : Command
    {
        private StartExpr value;

        public Execute(StartExpr value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return "(execute: " + value.ToString() + ")";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            return value.Collapse(story, storyNode);
        }

        public Value.Value Evaluate(Story story, StoryNode storyNode)
        {
            return value.Evaluate(story, storyNode);
        }
    }
}
