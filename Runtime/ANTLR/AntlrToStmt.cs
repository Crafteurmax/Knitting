using System;
using System.Collections.Generic;
using System.Globalization;
using Antlr4.Runtime.Tree;
using Runtime.ANTLR.Command;
using Runtime.ANTLR.Expression;
using Runtime.ANTLR.TwineValue;
using UnityEngine;
using ValueType = Runtime.ANTLR.Value.ValueType;

namespace Runtime.ANTLR
{
    public class AntlrToStmt : twine_harloweBaseVisitor<Statement>
    {
        private List<string> semanticsErrors;

        public AntlrToStmt(List<string> semanticsErrors)
        {
            this.semanticsErrors = semanticsErrors;
        }
        public override Statement VisitMultiplication(twine_harloweParser.MultiplicationContext context)
        {
            Expression.Expression leftExpression = (Expression.Expression)VisitExpression(context.expr(0));
            Expression.Expression rightExpression = (Expression.Expression)VisitExpression(context.expr(1));
            
            return new Multiplication(leftExpression, rightExpression);
        }

        public override Statement VisitAddition(twine_harloweParser.AdditionContext context)
        {
            Expression.Expression leftExpression = (Expression.Expression)VisitExpression(context.expr(0));
            Expression.Expression rightExpression = (Expression.Expression)VisitExpression(context.expr(1));
            
            return new Addition(leftExpression, rightExpression);
        }

        public override Statement VisitExprValue(twine_harloweParser.ExprValueContext context)
        {
            // Debug.Log("ExprValue");
            Statement statement = VisitValue(context.value());
            // Debug.Log("ExprValue result : " + statement);
            return statement;
        }

        public override Statement VisitDivision(twine_harloweParser.DivisionContext context)
        {
            
            Expression.Expression leftExpression = (Expression.Expression)VisitExpression(context.expr(0));
            Expression.Expression rightExpression = (Expression.Expression)VisitExpression(context.expr(1));
            
            return new Division(leftExpression, rightExpression);
        }

        public override Statement VisitBooleanLogic(twine_harloweParser.BooleanLogicContext context)
        {
            Expression.Expression leftExpression = (Expression.Expression)VisitExpression(context.expr(0));
            Expression.Expression rightExpression = (Expression.Expression)VisitExpression(context.expr(1));
            
            if(context.AND() != null) return new LogicAnd(leftExpression, rightExpression);
            if(context.OR() != null) return new LogicOr(leftExpression, rightExpression);

            throw new NotImplementedException(); // TODO
        }

        public override Statement VisitComparison(twine_harloweParser.ComparisonContext context)
        {
            Expression.Expression leftExpression = (Expression.Expression)VisitExpression(context.expr(0));
            Expression.Expression rightExpression = (Expression.Expression)VisitExpression(context.expr(1));

            if(context.IS() != null) return new Is(leftExpression, rightExpression);
            if(context.ISNOT() != null) return new IsNot(leftExpression, rightExpression);
            if(context.GT() != null) return new GreaterThan(leftExpression, rightExpression);
            if(context.LT() != null) return new LowerThan(leftExpression, rightExpression);
            if(context.GE() != null) return new GreaterEqual(leftExpression, rightExpression);
            if(context.LE() != null) return new LowerEqual(leftExpression, rightExpression);
            
            throw new NotImplementedException(); // TODO
        }

        public override Statement VisitSubstraction(twine_harloweParser.SubstractionContext context)
        {
            
            Expression.Expression leftExpression = (Expression.Expression)VisitExpression(context.expr(0));
            Expression.Expression rightExpression = (Expression.Expression)VisitExpression(context.expr(1));
            
            return new Substraction(leftExpression, rightExpression);
        }

        public override Statement VisitParentheses(twine_harloweParser.ParenthesesContext context)
        {
            Expression.Expression expression = (Expression.Expression)VisitExpression(context.expr());
            
            return new Parentheses(expression);
        }

        public override Statement VisitValue(twine_harloweParser.ValueContext context)
        {
            // Debug.Log("Value");
            ValueType type;

            if (context.array() != null)
            {
                type = ValueType.Array;
                throw new NotImplementedException(); //TODO
            }
            
            Statement statement = VisitLiteral(context.literal());
            // Debug.Log("Value result : " + statement);
            return statement;
        }

        public override Statement VisitLiteral(twine_harloweParser.LiteralContext context)
        {
            // Debug.Log("Literal");
            ValueType type;
            
            if (context.num() != null)
            {
                if (context.num().integer() != null) type = ValueType.Integer;
                else type = ValueType.Floating;
            }
            else if (context.@string()  != null) type = ValueType.String;
            else if (context.boolean() != null) type = ValueType.Boolean;
            else if (context.variable() != null) type = ValueType.Variable;
            else if (context.variable() != null) type = ValueType.Variable;
            else throw new NotImplementedException();

            string valueText = context.GetText();
            // Debug.Log(type + " : " + valueText);
            switch (type)
            {
                case ValueType.String:
                    return  new Value.Value(valueText.Substring(1,valueText.Length-2));
                case ValueType.Integer:
                    return new Value.Value(int.Parse(valueText));
                case ValueType.Floating:
                    return new Value.Value(float.Parse(valueText, CultureInfo.InvariantCulture));
                case ValueType.Boolean:
                    return new Value.Value(bool.Parse(valueText));
                case ValueType.Variable:
                    return VisitVariable(context.variable());
            }
            
            throw new NotImplementedException();
        }

        public override Statement VisitVariable(twine_harloweParser.VariableContext context)
        {
            return new Variable(context.GetText().Substring(1));
        }

        public override Statement VisitText(twine_harloweParser.TextContext context)
        {
            // Debug.Log("VisitText : \n" + context.GetText());
            return new Text(context.GetText());
        }

        public override Statement VisitCommand(twine_harloweParser.CommandContext context)
        {
            // Debug.Log("command");
            Statement statement = base.VisitCommand(context);
            // Debug.Log("command result : " + statement.ToString());
            return statement;
            
        }

        public override Statement VisitSet(twine_harloweParser.SetContext context)
        {
            // Debug.Log("Set");
            Statement statement = new Set((Variable)VisitVariable(context.variable()),((StartExpr)VisitStartExpr(context.startExpr())));
            // Debug.Log("Set result : " + statement.ToString());
            return statement;
        }

        public override Statement VisitPrint(twine_harloweParser.PrintContext context)
        {
            // Debug.Log("print");
            Statement statement = new Print((StartExpr)VisitStartExpr(context.startExpr()));
            // Debug.Log("print result : " + statement);
            return statement;
        }
        
        public override Statement VisitLowercase(twine_harloweParser.LowercaseContext context)
        {
            return new Lowercase((StartExpr)VisitStartExpr(context.startExpr()));
        }

        public override Statement VisitCond(twine_harloweParser.CondContext context)
        {
            List<StartExpr> startExprs = new List<StartExpr>();
            foreach (twine_harloweParser.StartExprContext expr in context.startExpr()) 
                startExprs.Add((StartExpr)VisitStartExpr(expr));
            
            return new Cond(startExprs);
        }

        public override Statement VisitExecute(twine_harloweParser.ExecuteContext context)
        {
            return new Execute((StartExpr)VisitStartExpr(context.startExpr()));
        }

        public override Statement VisitEither(twine_harloweParser.EitherContext context)
        {
            
            List<StartExpr> startExprs = new List<StartExpr>();
            foreach (twine_harloweParser.StartExprContext expr in context.startExpr()) 
                startExprs.Add((StartExpr)VisitStartExpr(expr));
            
            return new Either(startExprs);
        }

        public override Statement VisitStartExpr(twine_harloweParser.StartExprContext context)
        {
            // Debug.Log("StartExpr");
            Statement statement = new StartExpr((Expression.Expression)VisitExpression(context.expr()));
            // Debug.Log("StartExpr result : " + statement);
            return statement;
        }
        

        public Statement VisitExpression(twine_harloweParser.ExprContext context)
        {
            switch (context)
            {
                case twine_harloweParser.MultiplicationContext multiplicationContext:
                    return VisitMultiplication(multiplicationContext);
                case twine_harloweParser.DivisionContext divisionContext:
                    return VisitDivision(divisionContext);
                case twine_harloweParser.AdditionContext additionContext:
                    return VisitAddition(additionContext);
                case twine_harloweParser.SubstractionContext substractionContext:
                    return VisitSubstraction(substractionContext);
                case twine_harloweParser.ParenthesesContext parenthesesContext:
                    return VisitParentheses(parenthesesContext);
                case twine_harloweParser.ExprValueContext exprValueContext:
                    return VisitExprValue(exprValueContext);
                case twine_harloweParser.BooleanLogicContext booleanLogicContext:
                    return VisitBooleanLogic(booleanLogicContext);
                case twine_harloweParser.ComparisonContext comparaisonContext:
                    return VisitComparison(comparaisonContext);
                
                    
            }
            Debug.LogWarning("try to parse a non implemented expression");
            throw new NotImplementedException(); // TODO
        }
        
        public override Statement VisitStmt(twine_harloweParser.StmtContext context)
        {
            // Debug.Log("VisitStmt : enter");
            if (context.command() != null)
                return Visit(context.command());

            if (context.text() != null)
            {
                // Debug.Log("VisitStmt : exit to text");
                return Visit(context.text());
            }
                
            // Debug.Log("VisitStmt : exit to default");
            return base.VisitStmt(context);
        }

        public override Statement VisitConditional_block(twine_harloweParser.Conditional_blockContext context)
        {
            List<ConditionalBlock> conditionalBlocks = new List<ConditionalBlock>();

            conditionalBlocks.Add(VisitBlock(context.ifblock().startExpr(),context.ifblock().stmt()));

            foreach (twine_harloweParser.ElseifblockContext elseIfBlockContext in context.elseifblock())
                conditionalBlocks.Add(VisitBlock(elseIfBlockContext.startExpr(),elseIfBlockContext.stmt()));

            if (context.elseblock() != null)
            {
                List<Statement> elseBlockStmt = new List<Statement>();
                foreach (twine_harloweParser.StmtContext statement in context.elseblock().stmt()) elseBlockStmt.Add(VisitStmt(statement));
                
                return new ConditionalStatement(
                    conditionalBlocks,
                    new ConditionalBlock((StartExpr) null, new Passage(elseBlockStmt)));
            }
            return new ConditionalStatement(conditionalBlocks);
        }

        public ConditionalBlock VisitBlock(twine_harloweParser.StartExprContext startExprContext, twine_harloweParser.StmtContext[] stmtsContext)
        {
            List<Statement> stmts = new List<Statement>();
            foreach (twine_harloweParser.StmtContext statement in stmtsContext) stmts.Add(VisitStmt(statement));
            return new ConditionalBlock((StartExpr) VisitStartExpr(startExprContext), new  Passage(stmts));
        }

        public override Statement Visit(IParseTree tree)
        {
            var visited = base.Visit(tree); 
            // Debug.Log("visited : " + visited.ToString());
            return visited;
        }
    }
}
