using System.Collections.Generic;
using UnityEngine;

namespace Runtime.ANTLR
{
    public class AntlrToPassage : twine_harloweBaseVisitor<Passage>
    {
        public List<string> semanticsErrors;
        public override Passage VisitPassage(twine_harloweParser.PassageContext context)
        {
            Passage passage = new Passage();
            semanticsErrors = new List<string>();
            AntlrToStmt stmtVisitor = new AntlrToStmt(semanticsErrors);
            // Debug.Log(context.ChildCount);
            for (int i = 0; i < context.ChildCount - 1; i++)
            {
                // Debug.Log(context.GetChild(i));
                // if(context.GetChild(i).Eof() != null) continue;
                passage.AddStatement(stmtVisitor.Visit(context.GetChild(i)));
            }
            
            return passage;
        }
    }
}
