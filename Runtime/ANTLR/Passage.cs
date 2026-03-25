using System.Collections.Generic;
using UnityEngine;

namespace Runtime.ANTLR
{
    public class Passage
    {
        public List<Statement> statements;

        public Passage()
        {
            this.statements =  new List<Statement>();
        }

        public Passage(List<Statement> statements)
        {
            this.statements = statements;
        }

        public void AddStatement(Statement expr)
        {
            // Debug.Log("add statement");
            this.statements.Add(expr);
        }

        public string ToString()
        {
            string toReturn = "";
            // Debug.Log("statements count : " + statements.Count);
            for(int i = 0; i < this.statements.Count; i++)
            {
                // Debug.Log(statements[i]);
                toReturn += statements[i].ToString();
            }
            return toReturn;
        }

        public string Collapse(Story story, StoryNode storyNode)
        {
            string toReturn = "";
            // Debug.Log("Collapse statements count : " + statements.Count);
            for(int i = 0; i < this.statements.Count; i++)
            {
                // Debug.Log(statements[i]);
                toReturn += statements[i].Collapse(story, storyNode);
            }
            return toReturn;
        }
        
        
    }
}
