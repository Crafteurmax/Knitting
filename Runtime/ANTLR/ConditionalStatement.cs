using System.Collections.Generic;
using Runtime.ANTLR;
using Runtime.ANTLR.TwineValue;
using UnityEngine;

namespace Runtime.ANTLR.Command
{
    public class ConditionalStatement : Statement
    {
        List<ConditionalBlock> blocks;
        ConditionalBlock? defaultExit;

        public ConditionalStatement(List<ConditionalBlock> blocks, ConditionalBlock? defaultExit = null)
        {
            this.blocks = blocks;
            this.defaultExit = defaultExit;
        }

        public override string ToString()
        {
             string toReturn = "(if: " + blocks[0].condition.ToString() + ")[" + blocks[0].statement.ToString() + "]\n";

            for(int i = 1; i < blocks.Count; i++)
            {
                toReturn += "(else-if: " + blocks[i].condition.ToString() + ")[" + blocks[i].statement.ToString() + "]\n";
            }
            
            if (defaultExit == null)  return toReturn;
            
            toReturn += "(else: )[" + defaultExit?.statement.ToString() + "]\n";
            
            return toReturn;
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            foreach(ConditionalBlock block in blocks)
                if(block.condition.Evaluate(story, storyNode)) return block.statement.Collapse(story, storyNode);
            
            if (defaultExit == null) return "";
            
            return defaultExit?.statement.Collapse(story, storyNode);
            
        }
    }
}

public struct ConditionalBlock
{
    public StartExpr condition;
    public Passage statement;

    public ConditionalBlock(StartExpr visitStartExpr, Passage visitStmt)
    {
        condition = visitStartExpr;
        statement = visitStmt;
    }
}
