using System.Collections.Generic;
using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Cond : Command
    {
        private List<StartExpr> expressions;
        
        

        public Cond(List<StartExpr> expressions)
        {
            this.expressions = expressions;
        }

        public override string ToString()
        {
            string toReturn = "(cond: ";
            toReturn += expressions[0].ToString();
            for(int i = 1; i < expressions.Count; i++) toReturn += ", " + expressions[i].ToString();
            toReturn += ")";
            
            return toReturn;
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            for (int i = 0; i < expressions.Count - 1; i += 2)
            {
                if(expressions[i].Evaluate(story, storyNode)) return expressions[i+1].Collapse(story, storyNode);
            }
            
            return expressions[^1].Collapse(story, storyNode);
        }
    }
}
