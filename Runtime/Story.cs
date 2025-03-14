using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

public class Story : MonoBehaviour
{
    [SerializeField] private TextAsset twineText;
    [SerializeField] bool printVariableWhenSet;

    // Story datas
    private string title;
    private string ifid;
    private string format;
    private string formatVersion;
    private float zoom;
    private string start;
    private Dictionary<string, Color> tagColors = new Dictionary<string, Color>();

    // UserScript

    // UserStylesheet

    // Nodes
    Dictionary<string,StoryNode> nodes = new Dictionary<string, StoryNode>();
    Dictionary<string, string> nodeVariable = new Dictionary<string, string>();
    StoryNode currentNode;

    // Customisation
    private Dictionary<string, Color> possibleColors = new Dictionary<string, Color>();

    [System.Serializable]
    public class DescribedSprite
    {
        public string title;
        public Sprite sprite;
    }

    [SerializeField] bool usePath;
    [SerializeField] string path;
    [SerializeField] private List<DescribedSprite> sprites = new List<DescribedSprite>();


    // Debug 
    // public int index;

    // Start is called before the first frame update
    void Awake()
    {
        if (usePath) twineText = Resources.Load(path) as TextAsset;

        if (!twineText)
        {
            Debug.LogWarning("No twine file");
            return;
        }
        SetUpStory(twineText.text);
    }

    // ########## Setup ########## 

    public void SetUpStory(string text)
    {
        SetupPossiblesColors();
        string RGX_componentExtraction = @":: (?<title>.*)\n(?<data>((?:(?!^::).|\n)*))";
        foreach (Match component in Regex.Matches(text, RGX_componentExtraction, RegexOptions.Multiline))
        {
            if (!component.Success) continue;
            string componentTitle = component.Groups["title"].Value.Trim();
            string componentData = component.Groups["data"].Value.Trim();

            switch (componentTitle)
            {
                case "StoryTitle":
                    title = componentData;
                    break;
                case "StoryData":
                    ParseStoryData(componentData);
                    break;
                case "UserScript [script]":
                    // TODO
                    break;
                case "UserStylesheet [stylesheet]":
                    // TODO
                    break;
                default:
                    StoryNode node = new StoryNode(componentTitle, componentData, this);
                    nodes.Add(node.GetTitle(), node);
                    break;
            }

        }

        currentNode = nodes[start];
    }

    private void ParseStoryData(string data)
    {
        Regex RGX_parseData = new Regex(@"""(?<name>[^""]*)"": (({(?<colors>[^}]*)})|(""?(?<value>[^""\n,]*)""?))", RegexOptions.Multiline);

        MatchCollection datas = RGX_parseData.Matches(data);
        foreach (Match dataMatch in datas)
        {
            switch (dataMatch.Groups["name"].Value)
            {
                case "ifid":
                    ifid = dataMatch.Groups["value"].Value;
                    break ;
                case "format":
                    format = dataMatch.Groups["value"].Value;
                    break;
                case "format-version":
                    formatVersion = dataMatch.Groups["value"].Value;
                    break;
                case "start":
                    start = dataMatch.Groups["value"].Value;
                    break;
                case "zoom":
                    zoom = float.Parse(dataMatch.Groups["value"].Value.Replace('.',','));
                    break;
                case "tag-colors":
                    Regex RGX_parseColor = new Regex(@"[^""]*""(?<tag>[^""]*)"": ""(?<color>[^""]*)""");

                    MatchCollection pairs = RGX_parseColor.Matches(dataMatch.Groups["colors"].Value);
                    foreach (Match colorMatch in pairs)
                    {
                        string tag = colorMatch.Groups["tag"].Value;
                        Color color = possibleColors[colorMatch.Groups["color"].Value];
                        tagColors.Add(tag, color);
                    }
                    break;
            }
        }
    }

    private void SetupPossiblesColors()
    {
        possibleColors.Add("red", Color.red);
        possibleColors.Add("orange", new Color(1f, 0.498f, .0f));
        possibleColors.Add("yellow", Color.yellow);
        possibleColors.Add("green", Color.green);
        possibleColors.Add("blue", Color.blue);
        possibleColors.Add("purple", new Color(0.502f, 0f, 0.502f));
    }

    // ########## node variable ########## 

    public string GetVariable(string variableName)
    {
        string returnValue;
        bool isSet = nodeVariable.TryGetValue(variableName, out returnValue);
        //Assert.IsTrue(isSet, variableName + " is not defined");
        return returnValue;
    }

    public void SetVariable(string variableName, string value)
    {
        if(printVariableWhenSet) Debug.Log("set " + variableName + " to " + value);
        if (nodeVariable.ContainsKey(variableName)) nodeVariable[variableName] = value;
        else nodeVariable.Add(variableName, value);
    }

    // ########## Node changement ########## 

    public void NextNode()
    {
        List<NextNode> nextNodes = currentNode.GetNextNodes();
        SetNextNode(nextNodes[0].title);
    }

    public void ChooseNextNode(int index)
    {
        List<NextNode> nextNodes = currentNode.GetNextNodes();
        //Assert.IsTrue(index < nextNodes.Count, "Index out of range");
        SetNextNode(nextNodes[index].title);
    }

    public void SetNextNode(string title)
    {
        StoryNode next = null;
        nodes.TryGetValue(title, out next);
        //Assert.IsTrue(nodes.TryGetValue(title, out next), title + " doesn't existe");
        currentNode = next;
    }

    // ########## GETTER / SETTER ########## 

    public string GetTitle() { return title; }

    public string GetIfid() { return ifid; }

    public string GetFormat() { return format; }

    public string GetFormatVersion() { return formatVersion; }

    public float GetZoom() { return zoom; }

    public string GetStart() { return start; }

    public Dictionary<string, Color> GetTagColors() { return tagColors; }

    public StoryNode GetCurrentNode() { return currentNode; }

    public Sprite getSprite(string title)
    {
        foreach(DescribedSprite sprite in sprites) if (sprite.title == title) return sprite.sprite;
        return null;
    }

    // ########## Debug ########## 
    /*
    [ContextMenu("nextNode")]
    private void Debug_NextNode() 
    {
        NextNode();
        Debug.Log(currentNode.getText());
    }

    [ContextMenu("ChooseNextNode")]
    private void Debug_ChooseNextNode() 
    { 
        ChooseNextNode(index);
        Debug.Log(currentNode.getText());
    }


    [ContextMenu("SkipMultipleNextNode")]
    private void Debug_SkipMultipleNextNode()
    {
        for (int i = 0; i < index; i++)
        {
            NextNode();
            Debug.Log(currentNode.getText());
        }
    }
    */
}
