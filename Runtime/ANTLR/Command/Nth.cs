using System.Collections.Generic;
using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Nth : Command
    {
        private List<StartExpr> expressions;
        
        public Nth(List<StartExpr> expressions)
        {
            this.expressions = expressions;
        }

        public override string ToString()
        {
            string toReturn = "(nth: ";
            toReturn +=  expressions[0].ToString();
            for(int i = 1; i < expressions.Count; i++) toReturn += ", " + expressions[i].ToString();
            toReturn += ")";
            
            return toReturn;
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            int index = expressions[0].Evaluate(story, storyNode);
            
            index = (index - 1) % (expressions.Count - 1) + 1;
            
            return expressions[index].Collapse(story, storyNode);
        }
    }
}
