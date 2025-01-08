using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TestTools;

public class NodeTest
{
    private GameObject storyHolder = new GameObject();
    private Story story;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        story = storyHolder.AddComponent<Story>();
    }


    [Test]
    public void NodeTestWithTags()
    {
        Debug.Log("==========\nNodeTestWithTags\n==========");

        string componentTitle = "Cassandre_6 [PNJ QUESTION] {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentData = "(set: $A2 to '')\n" +
                                "Dis-moi machine savante. Quelle est la voie à suivre pour atteindre mes ambitions ?\n" +
                                "\n" +
                                "La Papesse[[A2HighPriestess]]\n" +
                                "L'Empereur[[A2Emperor]]\n" +
                                "L'Hermite[[A2Hermit]]\n" +
                                "Les Amoureux[[A2Lovers]]";

        StoryNode nodeA = new StoryNode(componentTitle, componentData, story);

        List<string> tags = new List<string>();
        tags.Add("PNJ");
        tags.Add("QUESTION");
        StoryNode nodeB = new StoryNode("Cassandre_6", tags, new Vector2Int(850,5350), new Vector2Int(100,100), componentData, story);

        nodeA.PrintNodeData();
        nodeB.PrintNodeData();

        Assert.IsTrue(nodeA.Equals(nodeB));
        Debug.Log("====================");
    }


    [Test]
    public void NodeTestWithoutTags()
    {
        Debug.Log("==========\nNodeTestWithoutTags\n==========");

        string componentTitle = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentData = "(set: $A2 to '')\n" +
                                "Dis-moi machine savante. Quelle est la voie à suivre pour atteindre mes ambitions ?\n" +
                                "\n" +
                                "La Papesse[[A2HighPriestess]]\n" +
                                "L'Empereur[[A2Emperor]]\n" +
                                "L'Hermite[[A2Hermit]]\n" +
                                "Les Amoureux[[A2Lovers]]";

        StoryNode nodeA = new StoryNode(componentTitle, componentData, story);

        List<string> tags = new List<string>();
        StoryNode nodeB = new StoryNode("Cassandre_6", tags, new Vector2Int(850, 5350), new Vector2Int(100, 100), componentData, story);

        nodeA.PrintNodeData();
        nodeB.PrintNodeData();

        Assert.IsTrue(nodeA.Equals(nodeB));

        Debug.Log("====================");
    }


    [Test]
    public void NodeTestDataParsing()
    {
        Debug.Log("==========\nNodeTestDataParsing\n==========");

        string componentTitle = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentData = "(set: $A2 to '')\n" +
                                "Dis-moi machine savante. Quelle est la voie à suivre pour atteindre mes ambitions ?\n" +
                                "\n" +
                                "[[La Papesse->A2HighPriestess]]\n" +
                                "[[A2Emperor<-L'Empereur]]\n" +
                                "[[L'Hermite->A2Hermit]]\n" +
                                "[[Les Amoureux->A2Lovers]]";


        string outputedText = "Dis-moi machine savante. Quelle est la voie à suivre pour atteindre mes ambitions ?";
        List<NextNode> goodNextNodes = new List<NextNode>();
        goodNextNodes.Add(new NextNode("A2HighPriestess", "La Papesse"));
        goodNextNodes.Add(new NextNode("A2Emperor", "L'Empereur"));
        goodNextNodes.Add(new NextNode("A2Hermit", "L'Hermite"));
        goodNextNodes.Add(new NextNode("A2Lovers", "Les Amoureux"));


        StoryNode nodeA = new StoryNode(componentTitle, componentData, story);

        Debug.Log(nodeA.getText());

        Assert.AreEqual(outputedText,nodeA.getText());

        List<NextNode> nextNodes = nodeA.GetNextNodes();
        Assert.AreEqual(nextNodes.Count, goodNextNodes.Count);
        for(int i = 0; i < goodNextNodes.Count; i++)
        {
            Assert.AreEqual(goodNextNodes[i].title, nextNodes[i].title);
            Assert.AreEqual(goodNextNodes[i].display, nextNodes[i].display);
        }


        Debug.Log("====================");
    }

    [Test]
    public void NodeTestSetVariable()
    {
        Debug.Log("==========\nNodeTestSetVariable\n==========");

        string componentTitle = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentData = "(set: $Variable to 'ThisIsAVariableValue')\n";

        StoryNode nodeA = new StoryNode(componentTitle, componentData, story);
        nodeA.getText();

        string value = story.GetVariable("Variable");

        Assert.AreEqual(value, "ThisIsAVariableValue");

        Debug.Log("====================");
    }

    [Test]
    public void NodeTestSimpleIf()
    {
        Debug.Log("==========\nNodeTestSimpleIf\n==========");

        string componentTitleA = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataA = "(set: $Variable to 'ThisIsAVariableValue')\n";
        StoryNode nodeA = new StoryNode(componentTitleA, componentDataA, story);


        string componentTitleB = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataB = "(if: $Variable is 'ThisIsAVariableValue')[Vous aviez raison, la Nouvelle Lune n’est que mensonge. Ma vie n’a cessé de s’améliorer depuis que je l’ai quittée.]\n" +
                                "(else-if: $Variable is 'ThisIsAnOtherVariableValue')[La vache... CETTE vache !! Elle est en train de tout me prendre ! Mon poste, mon parti, tous mes électeurs...]\n" +
                                "(else:)[La Nouvelle Lune m’a guidée dans l’ascension au sein de l’ordre. Je n’aurai jamais pu espérer une évolution plus fulgurante.]";
        StoryNode nodeB = new StoryNode(componentTitleB, componentDataB, story);

        string componentTitleC = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataC = "(set: $Variable to 'ThisIsAnOtherVariableValue')\n";
        StoryNode nodeC = new StoryNode(componentTitleC, componentDataC, story);

        string componentTitleD = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataD = "(set: $Variable to '')\n";
        StoryNode nodeD = new StoryNode(componentTitleD, componentDataD, story);
        nodeD.getText();

        Debug.Log(nodeB.getText());
        Assert.AreEqual("La Nouvelle Lune m’a guidée dans l’ascension au sein de l’ordre. Je n’aurai jamais pu espérer une évolution plus fulgurante.", nodeB.getText());
        
        nodeA.getText();
        string value = story.GetVariable("Variable");
        Assert.AreEqual(value, "ThisIsAVariableValue");

        Debug.Log(nodeB.getText());
        Assert.AreEqual("Vous aviez raison, la Nouvelle Lune n’est que mensonge. Ma vie n’a cessé de s’améliorer depuis que je l’ai quittée.", nodeB.getText());
        
        nodeC.getText();
        value = story.GetVariable("Variable");
        Assert.AreEqual(value, "ThisIsAnOtherVariableValue");

        Debug.Log(nodeB.getText());
        Assert.AreEqual("La vache... CETTE vache !! Elle est en train de tout me prendre ! Mon poste, mon parti, tous mes électeurs...", nodeB.getText());

        Debug.Log("====================");
    }

    [Test]
    public void NodeTestUnless()
    {
        Debug.Log("==========\nNodeTestUnless\n==========");


        string componentTitleA = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataA = "(set: $Variable to '')\n";
        StoryNode nodeA = new StoryNode(componentTitleA, componentDataA, story);

        string componentTitleB = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataB = "(unless: $Variable is 'ThisIsAVariableValue')[Vous aviez raison, la Nouvelle Lune n’est que mensonge.]";
        StoryNode nodeB = new StoryNode(componentTitleB, componentDataB, story);

        string componentTitleC = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataC = "(set: $Variable to 'ThisIsAVariableValue')\n";
        StoryNode nodeC = new StoryNode(componentTitleC, componentDataC, story);

        nodeA.getText();
        Debug.Log(nodeB.getText());
        Assert.AreEqual("Vous aviez raison, la Nouvelle Lune n’est que mensonge.", nodeB.getText());

        nodeC.getText();
        Debug.Log(nodeB.getText());
        Assert.AreEqual("", nodeB.getText());

        Debug.Log("====================");
    }

    [Test]
    public void NodeTestEither()
    {
        Debug.Log("==========\nNodeTestEither\n==========");


        string componentTitle = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentData = "this test is (either: 'good', 'bad', 'ug,ly').";
        StoryNode node = new StoryNode(componentTitle, componentData, story);

        int[] count = {0,0,0};

        for (int i = 0; i < 1000; i++)
        {
            Debug.Log(node.getText());
            switch (node.getText())
            {
                case "this test is good.":
                    count[0]++;
                    break;
                case "this test is bad.":
                    count[1]++;
                    break;
                case "this test is ug,ly.":
                    count[2]++;
                    break;
            }
        }

        Debug.Log(node.getText());
        Assert.AreEqual(1000, count[0] + count[1] + count[2]);
        Assert.Greater(count[0], 300);
        Assert.Greater(count[1], 300);
        Assert.Greater(count[2], 300);

        Debug.Log("====================");
    }

    [Test]
    public void NodeTestCond()
    {
        Debug.Log("==========\nNodeTestCond\n==========");


        string componentTitleA = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataA = "(set: $Variable to 'false')\n";
        StoryNode nodeA = new StoryNode(componentTitleA, componentDataA, story);

        string componentTitleB = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataB = "Your (cond: $Variable, \"gasps of triumph\", \"wheezes of defeat\") drown out all other noise.";
        StoryNode nodeB = new StoryNode(componentTitleB, componentDataB, story);

        string componentTitleC = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataC = "(set: $Variable to 'true')\n";
        StoryNode nodeC = new StoryNode(componentTitleC, componentDataC, story);

        nodeA.getText();
        Debug.Log(nodeB.getText());
        Assert.AreEqual("Your wheezes of defeat drown out all other noise.", nodeB.getText());

        nodeC.getText();
        Debug.Log(nodeB.getText());
        Assert.AreEqual("Your gasps of triumph drown out all other noise.", nodeB.getText());

        Debug.Log("====================");
    }

    [Test]
    public void NodeTestNth()
    {
        Debug.Log("==========\nNodeTestNth\n==========");


        string componentTitleA = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataA = "(set: $Variable to '1')";
        StoryNode nodeA = new StoryNode(componentTitleA, componentDataA, story);

        string componentTitleB = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataB = "(nth: $Variable, \"Hi!\", \"Hello again!\", \"Oh, it's you!\", \"Hey!\")";
        StoryNode nodeB = new StoryNode(componentTitleB, componentDataB, story);

        string componentTitleC = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataC = "(set: $Variable to '3')";
        StoryNode nodeC = new StoryNode(componentTitleC, componentDataC, story);

        string componentTitleD = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentDataD = "(set: $Variable to '5')";
        StoryNode nodeD = new StoryNode(componentTitleD, componentDataD, story);

        nodeA.getText();
        // Debug.Log(nodeB.getText());
        Assert.AreEqual("Hi!", nodeB.getText());

        nodeC.getText();
        // Debug.Log(nodeB.getText());
        Assert.AreEqual("Oh, it's you!", nodeB.getText());

        nodeD.getText();
        // Debug.Log(nodeB.getText());
        Assert.AreEqual("Hi!", nodeB.getText());

        Debug.Log("====================");
    }
    
    [Test]
    public void NodeTestPrint()
    {
        Debug.Log("==========\nNodeTestPrint\n==========");

        string componentTitle = "Cassandre_6 {\"position\":\"850,5350\",\"size\":\"100,100\"}";
        string componentData = "(set: $Variable to 'successful')this is a (print: $Variable) test !";
        StoryNode node = new StoryNode(componentTitle, componentData, story);

        Debug.Log(node.getText());
        Assert.AreEqual("this is a successful test !", node.getText());

        Debug.Log("====================");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*
    [UnityTest]
    public IEnumerator NodeTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
    */
}

/*
 'zefef' is'ThisIsAVariableValue'
'fvc' is not   'ThisIsAVariableValue'
'cvd'contains'ThisIsAVariableValue'
'' does not contain 'ThisIsAVariableValue'
'' is in 'ThisIsAVariableValue'
'' > 'ThisIsAVariableValue'
'' >= 'ThisIsAVariableValue'
'' < 'ThisIsAVariableValue'
'' <= 'ThisIsAVariableValue'
'' and 'ThisIsAVariableValue'
'' or 'ThisIsAVariableValue'
'true'
'false'
 */