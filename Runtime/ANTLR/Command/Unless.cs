using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Unless : Command
    {
        private StartExpr value;
        private Passage passage;

        public Unless(StartExpr value, Passage passage)
        {
            this.value = value;
            this.passage = passage;
        }

        public override string ToString()
        {
            return "(unless: " + value.ToString() + ")[" + passage.ToString() + "]";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            if(!value.Evaluate(story, storyNode)) return passage.Collapse(story, storyNode);
            return "";
        }
    }
}
