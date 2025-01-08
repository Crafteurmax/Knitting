using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static ParsingTools;

public class ParsingToolsTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void ParsingToolsTestFindInWord()
    {
        string text = "( kefoi \")\"\')\') ) ";

        int index = ParsingTools.FindInWord(text, ')');
        Assert.AreEqual(14, index);

        index = ParsingTools.FindInWord(text, ')', index + 1);
        Assert.AreEqual(16, index);

        index = ParsingTools.FindInWord(text, '7');
        Assert.AreEqual(-1, index);

        index = ParsingTools.FindInWord(text, ')',17);
        Assert.AreEqual(-1, index);
    }

    [Test]
    public void ParsingToolsTestCarefullSplit()
    {
        string text = ",I m a list of values, separated, by coma, \"but please, don't cut me\", 'me, neither'";
        List<string> goodSplit = new List<string>();
        goodSplit.Add("");
        goodSplit.Add("I m a list of values");
        goodSplit.Add(" separated");
        goodSplit.Add(" by coma");
        goodSplit.Add(" \"but please, don't cut me\"");
        goodSplit.Add(" 'me, neither'");

        Assert.AreEqual(goodSplit, ParsingTools.CarefullSplit(text,','));

        goodSplit.Add("");
        text += ",";
        Assert.AreEqual(goodSplit, ParsingTools.CarefullSplit(text, ','));
    }

    [Test]
    public void ParsingToolsTestGetBetween()
    {
        string text = ",I m a list of (values, separated, by coma, \"but please, don't cut me\", 'me, neither')";
        
        Assert.AreEqual("(values, separated, by coma, \"but please, don't cut me\", 'me, neither')", ParsingTools.GetBetween(text,'(',')').Value);
    }


    [Test]
    public void ParsingToolsGetCommande()
    {
        string text = "something (a macro)";

        Assert.AreEqual("(a macro)", ParsingTools.GetCommande(text).Value);

        text += "[a hook]";

        Assert.AreEqual("(a macro)[a hook]", ParsingTools.GetCommande(text).Value);
        Assert.AreEqual("a macro", ParsingTools.GetCommande(text).Groups["commande"]);
        Assert.AreEqual("a hook", ParsingTools.GetCommande(text).Groups["text"]);
    }

    [Test]
    public void ParsingToolsGetAllCommandes()
    {
        string text = "something (a macro) \n" +
                        "(an other macro)[with an hook]\n" +
                        "a trap\n" +
                        "HA HA !!! (the final macro)[with a final hook]";

        List<ParsingResult> goodParsingResults = new List<ParsingResult>();
        goodParsingResults.Add(ParsingTools.GetCommande("(a macro) \n"));
        goodParsingResults.Add(ParsingTools.GetCommande("(an other macro)[with an hook]\n"));
        goodParsingResults.Add(ParsingTools.GetCommande("(the final macro)[with a final hook]"));

        Assert.AreEqual(goodParsingResults, ParsingTools.GetAllCommandes(text));
    }

    [Test]
    public void ParsingToolsGetOpposit()
    {
        string text = "something (a macro) \n" +
                        "(an other macro)[with an hook]\n" +
                        "a trap\n" +
                        "HA HA !!! (the final macro)[with a final hook]";

        List<ParsingResult> complement = ParsingTools.GetAllCommandes(text);

        List<string> expected = new List<string>();
        expected.Add("something ");
        expected.Add(" \n");
        expected.Add("\na trap\nHA HA !!! ");

        Assert.AreEqual(expected, ParsingTools.GetOpposit(text, complement));

        text += "SURPRISE !!!";
        expected.Add("SURPRISE !!!");
        Assert.AreEqual(expected, ParsingTools.GetOpposit(text, complement));

    }
}
