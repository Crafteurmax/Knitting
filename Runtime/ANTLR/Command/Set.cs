using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Set : Command
    {
        private Variable variable;
        private StartExpr value;

        public Set(Variable variable, StartExpr value)
        {
            this.variable = variable;
            this.value = value;
        }

        public override string ToString()
        {
            return "(set: " + variable.ToString() + " to " + value.ToString() + ")";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            // Debug.Log("Collapse Set");
            // Debug.Log(value.Evaluate(story,storyNode).ToString());
            story.SetVariable(variable.id, value.Evaluate(story, storyNode));
            return ""; // Invisible function only do backgrounds actions
        }
    }
}
