using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

public class StoryTest
{
    private GameObject storyHolder = new GameObject();
    private Story story;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        story = storyHolder.AddComponent<Story>();

        Debug.Log(TestContext.CurrentContext.TestDirectory);
        string filePath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", @"Assets\Tests\Runtime\TestData\TestTwine.txt"));
        StreamReader sr = new StreamReader(filePath);
        string fileText = sr.ReadToEnd();
        sr.Close();

        Assert.IsNotEmpty(fileText,"could't open test file");
        story.SetUpStory(fileText);
    }

    [Test]
    public void StoryTestSetUpData()
    {
        Debug.Log("==========\nStoryTestSetUpData\n==========");
        Assert.AreEqual("Testing Story", story.GetTitle());
        Assert.AreEqual("66DECB09-30F9-4695-BF93-DDD630A721F9", story.GetIfid());
        Assert.AreEqual("Harlowe", story.GetFormat());
        Assert.AreEqual("3.3.9", story.GetFormatVersion());
        Assert.AreEqual("Start", story.GetStart());
        Assert.AreEqual(1, story.GetZoom());

        Dictionary<string, Color> goodColor = new Dictionary<string, Color>();
        goodColor.Add("START", Color.green);
        goodColor.Add("PNJ", Color.blue);
        goodColor.Add("VOYANTE", Color.red);

        Assert.AreEqual(goodColor, story.GetTagColors());

        Debug.Log("====================");
    }

    [Test]
    public void StoryTestNextNode()
    {
        Debug.Log("==========\nStoryTestNextNode\n==========");
        Assert.AreEqual("Start", story.GetCurrentNode().GetTitle());
        story.NextNode();
        Assert.AreEqual("Alex_4", story.GetCurrentNode().GetTitle());
        story.ChooseNextNode(2);
        Assert.AreEqual("S1Hermit", story.GetCurrentNode().GetTitle());
        story.SetNextNode("Start");
        Assert.AreEqual("Start", story.GetCurrentNode().GetTitle());

        Debug.Log("====================");
    }


    /*
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator StoryTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}