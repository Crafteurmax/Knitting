using System.Collections;
using NUnit.Framework;
using Runtime.ANTLR;
using Runtime.ANTLR.Command;
using Runtime.ANTLR.TwineValue;
using UnityEngine;
using UnityEngine.TestTools;

public class ANTLRParsingTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void ANTLRParsingTest_Multiplication()
    {
        Debug.Log("==========\nANTLRParsingTest_Multiplication\n==========");
        
        Passage intPassage = ANTLRWrapper.getPassagesTree("(execute: 10 * 10)");
        int intValue = ((Execute)intPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(100, intValue);
        
        Passage floatPassage = ANTLRWrapper.getPassagesTree("(execute: 10 * 10.5)");
        float floatValue = ((Execute)floatPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(105f, floatValue);
        
        floatPassage = ANTLRWrapper.getPassagesTree("(execute: 10.5 * 10)");
        floatValue = ((Execute)floatPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(105f, floatValue);
        
        Passage boolFalsePassage = ANTLRWrapper.getPassagesTree("(execute: false * 10)");
        int boolFalseValue = ((Execute)boolFalsePassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(0, boolFalseValue);
        
        boolFalsePassage = ANTLRWrapper.getPassagesTree("(execute: 10 * false)");
        boolFalseValue = ((Execute)boolFalsePassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(0, boolFalseValue);
        
        Passage boolTruePassage = ANTLRWrapper.getPassagesTree("(execute: true * 10)");
        int boolTrueValue = ((Execute)boolTruePassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(10, boolTrueValue);
        
        Debug.Log("====================");
    }
    
    [Test]
    public void ANTLRParsingTest_Division()
    {
        Debug.Log("==========\nANTLRParsingTest_Division\n==========");
        
        Passage intPassage = ANTLRWrapper.getPassagesTree("(execute: 6 / 4)");
        int intValue = ((Execute)intPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(1, intValue);
        
        Passage floatPassage = ANTLRWrapper.getPassagesTree("(execute: 10.0 / 10)");
        float floatValue = ((Execute)floatPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(1f, floatValue);
        
        floatPassage = ANTLRWrapper.getPassagesTree("(execute: 5.0 / 2.0)");
        floatValue = ((Execute)floatPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(2.5f, floatValue);
        
        Debug.Log("====================");
    }
    
    [Test]
    public void ANTLRParsingTest_Addition()
    {
        Debug.Log("==========\nANTLRParsingTest_Addition\n==========");
        
        Passage intPassage = ANTLRWrapper.getPassagesTree("(execute: 10 + 10)");
        int intValue = ((Execute)intPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(20, intValue);
        
        Passage floatPassage = ANTLRWrapper.getPassagesTree("(execute: 10 + 10.5)");
        float floatValue = ((Execute)floatPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(20.5f, floatValue);
        
        floatPassage = ANTLRWrapper.getPassagesTree("(execute: 10.5 + 10)");
        floatValue = ((Execute)floatPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(20.5f, floatValue);
        
        Passage stringPassage = ANTLRWrapper.getPassagesTree("(execute: \"toto\" + 10.5 + 10)");
        string stringValue = ((Execute)stringPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual("toto10.510", stringValue);
        
        Debug.Log("====================");
    }
    
    
    [Test]
    public void ANTLRParsingTest_Subtraction()
    {
        Debug.Log("==========\nANTLRParsingTest_Subtraction\n==========");
        
        Passage intPassage = ANTLRWrapper.getPassagesTree("(execute: 10 - 10)");
        int intValue = ((Execute)intPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(0, intValue);
        
        Passage floatPassage = ANTLRWrapper.getPassagesTree("(execute: 10 - 10.5)");
        float floatValue = ((Execute)floatPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(-0.5f, floatValue);
        
        floatPassage = ANTLRWrapper.getPassagesTree("(execute: 10.5 - 10)");
        floatValue = ((Execute)floatPassage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(0.5f, floatValue);
        
        Debug.Log("====================");
    }
    
    
    [Test]
    public void ANTLRParsingTest_Parentheses()
    {
        Debug.Log("==========\nANTLRParsingTest_Parentheses\n==========");
        
        Passage passage = ANTLRWrapper.getPassagesTree("(execute: (10 + 10) * 2)");
        int value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(40, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 2 * (10 + 10))");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(40, value);
        
        Debug.Log("====================");
    }
    
    
    [Test]
    public void ANTLRParsingTest_BooleanLogic()
    {
        Debug.Log("==========\nANTLRParsingTest_BooleanLogic\n==========");
        
        Passage passage = ANTLRWrapper.getPassagesTree("(execute: true and true)");
        bool value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: false and true)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: false and false)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: false and false)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: true or true)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: false or true)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: true or false)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: false or false)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        Debug.Log("====================");
    }
    [Test]
    public void ANTLRParsingTest_Comparison()
    {
        Debug.Log("==========\nANTLRParsingTest_Comparison\n==========");
        
        Passage passage = ANTLRWrapper.getPassagesTree("(execute: 10 is 10)");
        bool value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 is 20)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 is not 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 is not 20)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);

        passage = ANTLRWrapper.getPassagesTree("(execute: 10 > 20)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 > 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 20 > 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 < 20)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 < 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 20 < 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 >= 20)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 >= 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 20 >= 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 <= 20)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 10 <= 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(true, value);
        
        passage = ANTLRWrapper.getPassagesTree("(execute: 20 <= 10)");
        value = ((Execute)passage.statements[0]).Evaluate(null, null);
        Assert.AreEqual(false, value);
        
        Debug.Log("====================");
    }

    
    
    
}
