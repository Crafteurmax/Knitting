using System.Collections;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Runtime.ANTLR;

public static class ANTLRWrapper
{
    private static twine_harloweParser getParser(string input)
    {
        ICharStream inputStream = new AntlrInputStream(input);
        twine_harloweLexer lexer = new twine_harloweLexer(inputStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        twine_harloweParser parser = new twine_harloweParser(tokens);

        
        return parser;
    }

    public static Passage getPassagesTree(string textToParse)
    {
        twine_harloweParser  parser = getParser(textToParse);
        IParseTree tree = parser.passage();
        // Debug.Log("parsed successfully");
        
        AntlrToPassage passageVisitor = new AntlrToPassage();
        return passageVisitor.Visit(tree);
    }
}
