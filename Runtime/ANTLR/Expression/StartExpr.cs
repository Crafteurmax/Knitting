using System.Collections;
using System.Collections.Generic;
using Runtime.ANTLR;
using Runtime.ANTLR.Expression;
using Runtime.ANTLR.TwineValue;
using Runtime.ANTLR.Value;
using UnityEngine;

public class StartExpr : Statement
{
    Expression rootExpression;

    public StartExpr(Expression rootExpression)
    {
        // Debug.Log("StartExpression construct : " + rootExpression);
        // Debug.Log(rootExpression.GetType() == null ? "root is null" : "root is not null");
        this.rootExpression = rootExpression;
    }

    public override string ToString()
    {
        // Debug.Log("StartExpr ToString : " + rootExpression);
        return rootExpression.ToString();
    }

    public override string Collapse(Story story, StoryNode storyNode)
    {
        // Debug.Log("collapse StartExpr");
        return Evaluate(story, storyNode).Collapse(story, storyNode);
    }

    public Value Evaluate(Story story, StoryNode storyNode)
    {
        // Debug.Log("Evaluate StartExpr");
        return rootExpression.Evaluate(story, storyNode);
    }
}
