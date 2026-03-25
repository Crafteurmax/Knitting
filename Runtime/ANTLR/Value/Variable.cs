using System.Runtime.CompilerServices;
using UnityEngine;

namespace Runtime.ANTLR.TwineValue
{
    public class Variable : Expression.Expression
    {
        public string id;
        public Variable(string id)
        {
            this.id = id;
        }

        public override string ToString()
        {
            // Debug.Log("Variable ToString");
            return "$" + id;
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            // Debug.Log("collapse Variable");
            return Evaluate(story, storyNode).Collapse(story, storyNode);
        }

        public override Value.Value Evaluate(Story story, StoryNode storyNode)
        {
            // Debug.Log("evaluate Variable");
            return story.GetVariable(id);
        }
    }
}
