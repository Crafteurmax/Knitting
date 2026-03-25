using System.Collections.Generic;
using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class Either : Command
    {
        private List<StartExpr> expressions;
        
        

        public Either(List<StartExpr> expressions)
        {
            this.expressions = expressions;
        }

        public override string ToString()
        {
            string toReturn = "(either: ";
            toReturn +=  expressions[0].ToString();
            for(int i = 1; i < expressions.Count; i++) toReturn += ", " + expressions[i].ToString();
            toReturn += ")";
            
            return toReturn;
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            return expressions[Random.Range(0,expressions.Count)].Collapse(story, storyNode);
        }
    }
}
