using Codice.Client.BaseCommands;
using Codice.Client.Common.TreeGrouper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;
using static ParsingTools;
using Random = UnityEngine.Random;

public class NextNode
{
    public string title;
    public string display;

    public NextNode(string _title, string _display)
    {
        title = _title;
        display = _display;
    }
}
public class StoryNode
{
    
    private string title;
    private List<string> tags = new List<string>();
    private Vector2Int position;
    private Vector2Int size;

    private string rawText;
    private List<NextNode> nextNodes = new List<NextNode>();

    private Story parentStrory;

    private Sprite sprite = null;

    public StoryNode(string _title, string _data, Story _parentStrory)
    {
        Assert.IsNotNull(_parentStrory);

        ParseTitle(_title);
        ParseData(_data);
        parentStrory = _parentStrory;
    }

    public StoryNode(string _title, List<string> _tags, Vector2Int _position, Vector2Int _size, string _data, Story _parentStrory)
    {
        title = _title;
        tags = _tags;
        position = _position;
        size = _size;
        parentStrory = _parentStrory;

        ParseData(_data);
    }


    // ########## SETUP ########## 
    private void ParseTitle(string _title)
    {
        Regex regex = new Regex(@"(?<title>[^ ]*)( \[)?(?<tags>[^]]*)(\])? {""position"":""(?<posX>\d*),(?<posY>\d*)"",""size"":""(?<sizeX>\d*),(?<sizeY>\d*)""}");

        Match match = regex.Match(_title);
        Assert.IsTrue(match.Success, _title + " is not a valid title");

        title = match.Groups["title"].Value;
        
        string tagsString = match.Groups["tags"].Value;
        if(tagsString.Length != 0) foreach (string tag in tagsString.Split(" ")) tags.Add(tag);

        position.x = int.Parse(match.Groups["posX"].Value);
        position.y = int.Parse(match.Groups["posY"].Value);

        size.x = int.Parse(match.Groups["sizeX"].Value);
        size.y = int.Parse(match.Groups["sizeY"].Value);
    }

    private void ParseData(string _data)
    {
        Regex destinationChoice = new Regex(@"^\[\[(((?<textChoicePrefix>.*)->)|)(?<destination>[^]<]*)((<-(?<textChoiceSufix>.*))|)]]");
        foreach (string line in _data.Split("\n"))
        {
            Match match = destinationChoice.Match(line);
            if (!match.Success) 
            {
                rawText += line + "\n";
                continue;
            }

            nextNodes.Add(new NextNode(match.Groups["destination"].Value, match.Groups["textChoicePrefix"].Success? match.Groups["textChoicePrefix"].Value: match.Groups["textChoiceSufix"].Value));
        }

        rawText = rawText.Trim();
    }

    public void PrintNodeData()
    {
        string output = "Title : " + title + '\n';

        output += "Tags : ";

        if (tags.Count == 0) output += "NO TAGS\n";
        else
        {
            output += "[";
            foreach (string tag in tags) output += tag + " ";
            output = output.Trim();
            output += "]\n";
        }

        output += "Position : " + position;
        output += "\nSize : " + size;

        Debug.Log(output);
    }

    override public string ToString() { return title; }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(StoryNode)) return false;
        StoryNode node = (StoryNode)obj;

        if(node == null) return false;

        if(node.title != title) return false;
        if(node.position != position) return false;
        if(node.size != size) return false;
        if(node.tags.Count != tags.Count) return false;

        for(int i = 0; i < tags.Count; i++) if (node.tags[i] != tags[i]) return false;

        return true;
    }


    // ########## Execute commandes ########## 
    private enum COMMAND_TYPE
    {
        SET,
        IF,
        ELSEIF,
        ELSE,
        SOUND,
        UNLESS,
        EITHER,
        COND,
        NTH,
        PRINT,
        IMAGE,
        DEFAULT
    }

    public string getText()
    {
        return ParseText(rawText);
    }

    private string ParseText(string textToParse)
    {
        //Regex patternExtract = new Regex(@"\((?<commande>[^)]*)\)\n?([^[\n]*\[(?<text>[^]]*)\]\n?|)");
        bool isConditionAlreadyValide = false;

        List<ParsingResult> commands = ParsingTools.GetAllCommandes(textToParse);

        List<string> notCommands = ParsingTools.GetOpposit(textToParse,commands);

        string parsedText = "";

        for(int i = 0; i < notCommands.Count; i++)
        {
            string text = ReplaceVariablesByValues(notCommands[i], "");
            if (text.Length > 0 && text[0] == '\n') text = text.Substring(1);
            parsedText += text;

            if (i < commands.Count)
            {
                ParsingResult match = commands[i];

                COMMAND_TYPE commandType = GetCommandType(match.Groups["commande"]);
                switch (commandType)
                {
                    case COMMAND_TYPE.SET:
                        // Regex patternExtractSetVariable = new Regex(@"set: \$(?<variableName>.*) to ('|"")(?<value>.*)('|"")");
                        ParsingResult variableMatch = ParsingTools.GetSet(match.Groups["commande"]);
                        Assert.IsTrue(variableMatch.succes);

                        // Debug.Log("[" + variableMatch.Groups["variableName"] + "]");

                        parentStrory.SetVariable(variableMatch.Groups["variableName"], variableMatch.Groups["value"]);
                        break;
                    case COMMAND_TYPE.IF:
                        isConditionAlreadyValide = false;

                        if (TestCondition(match.Groups["commande"]))
                        {
                            isConditionAlreadyValide = true;
                            parsedText += ParseText(match.Groups["text"]);
                        }

                        break;
                    case COMMAND_TYPE.ELSEIF:
                        if (isConditionAlreadyValide) break;

                        if (TestCondition(match.Groups["commande"]))
                        {
                            isConditionAlreadyValide = true;
                            parsedText += ParseText(match.Groups["text"]);
                        }
                        break;
                    case COMMAND_TYPE.ELSE:
                        if (isConditionAlreadyValide) break;
                        parsedText += ParseText(match.Groups["text"]);
                        break;
                    case COMMAND_TYPE.SOUND:

                        PlaySound(match.Groups["commande"]);
                        break;
                    case COMMAND_TYPE.UNLESS:
                        if (!TestCondition(match.Groups["commande"])) parsedText += ParseText(match.Groups["text"]);
                        break;
                    case COMMAND_TYPE.EITHER:
                        string formatedText = ReplaceVariablesByValues(match.Groups["commande"], "");
                        // Regex RGX_ExtractChoices = new Regex(@"'(?<choice>[^']*)'");

                        List<ParsingResult> matches = ParsingTools.GetAllWords(formatedText);
                        parsedText += matches[Random.Range(0, matches.Count)].Groups["Inside"];

                        break;
                    case COMMAND_TYPE.COND:
                        string arguments = match.Groups["commande"].Remove(0, 5);

                        List<string> splitedArguments = ParsingTools.CarefullSplit(arguments,',');

                        bool foundValidCondition = false;
                        
                        for(int j  = 0; j < splitedArguments.Count /2; j+=2)
                        {
                            if (!foundValidCondition && TestCondition(splitedArguments[j])) 
                            {
                                parsedText += ReplaceVariablesByValues(splitedArguments[j + 1], "").Substring(2,splitedArguments[j +1].Length - 3); 
                                foundValidCondition = true;
                                break;
                            }

                        }

                        if(!foundValidCondition) parsedText += ReplaceVariablesByValues(splitedArguments[splitedArguments.Count - 1], "")
                                .Substring(2, splitedArguments[splitedArguments.Count - 1].Length - 3);

                        break;
                    case COMMAND_TYPE.NTH:
                        string arguments2 = match.Groups["commande"].Remove(0, 4);

                        List<string> splitedArguments2 = ParsingTools.CarefullSplit(ReplaceVariablesByValues(arguments2, ""),',');

                        Assert.IsTrue(int.TryParse(splitedArguments2[0], out int nth), splitedArguments2[0] + " is not a number");

                        // Debug.Log("splitedArguments2.Count : " + splitedArguments2.Count);
                        nth = (nth - 1) % (splitedArguments2.Count - 1) + 1;
                        // Debug.Log("nth : " + nth);
                        parsedText += ReplaceVariablesByValues(splitedArguments2[nth], "").Substring(2, splitedArguments2[nth].Length - 3);


                        break;
                    case COMMAND_TYPE.PRINT:
                        string argument = match.Groups["commande"].Remove(0, 6);
                        argument = argument.Trim();
                        parsedText += ReplaceVariablesByValues(argument, "");
                        break;
                    case COMMAND_TYPE.IMAGE:
                        string imageName = ParsingTools.GetWord(match.Groups["commande"],6).Groups["Inside"];
                        sprite = parentStrory.getSprite(imageName); 
                        break;
                    case COMMAND_TYPE.DEFAULT:
                        // Debug.Log("false positve");
                        parsedText += ReplaceVariablesByValues(match.Value, "");
                        break;
                }
            }
        }


        return parsedText;
    }

    private COMMAND_TYPE GetCommandType(string command)
    {
        if (command.StartsWith("set:")) return COMMAND_TYPE.SET;
        if (command.StartsWith("if:")) return COMMAND_TYPE.IF;
        if (command.StartsWith("else-if:")) return COMMAND_TYPE.ELSEIF;
        if (command.StartsWith("else:")) return COMMAND_TYPE.ELSE;
        if (command.StartsWith("sound:")) return COMMAND_TYPE.SOUND;
        if (command.StartsWith("unless:")) return COMMAND_TYPE.UNLESS;
        if (command.StartsWith("either:")) return COMMAND_TYPE.EITHER;
        if (command.StartsWith("cond:")) return COMMAND_TYPE.COND;
        if (command.StartsWith("nth:")) return COMMAND_TYPE.NTH;
        if (command.StartsWith("print:")) return COMMAND_TYPE.PRINT;
        if (command.StartsWith("image:")) return COMMAND_TYPE.IMAGE;
        return COMMAND_TYPE.DEFAULT;
    }

    private bool TestCondition(string condition)
    {
        // replace variable by real value
        condition = ReplaceVariablesByValues(condition, "'");

        // evaluate
        Regex RGX_extractValues = new Regex(@"((('|"")(?<leftValue>[^'""]*?)('|"") *(?<operator>[^'""\n]*?) *('|"")(?<rightValue>[^'""]*?)('|""))|('|"")(true|false)('|"")|not *('|"")(?<uniqueValue>[^'""]*?)('|""))");
        Match match = RGX_extractValues.Match(condition);

        Assert.IsTrue(match.Success, "syntaxe error in condition : " + condition);

        if (condition.StartsWith("not"))
        {
            bool isUniqueValueABool = match.Groups["uniqueValue"].Value == "true" || match.Groups["uniqueValue"].Value == "false";
            Assert.IsTrue(isUniqueValueABool, "invalide argument " + match.Groups["uniqueValue"].Value + "is not a boolean");

            return match.Groups["uniqueValue"].Value == "false";
        }
        if (match.Value == "'true'" || match.Value == "'false'") return match.Value == "'true'";

        bool isLeftValueANumber = float.TryParse(match.Groups["leftValue"].Value, out float leftValue);
        bool isRightValueANumber = float.TryParse(match.Groups["rightValue"].Value, out float rightValue);

        bool isLeftValueABool = !isLeftValueANumber && (match.Groups["leftValue"].Value == "true" || match.Groups["leftValue"].Value == "false");
        bool isRightValueABool = !isRightValueANumber && (match.Groups["rightValue"].Value == "true" || match.Groups["rightValue"].Value == "false");


        switch (match.Groups["operator"].Value)
        {
            case "is":
                return match.Groups["leftValue"].Value == match.Groups["rightValue"].Value;
            case "is not":
                return match.Groups["leftValue"].Value != match.Groups["rightValue"].Value;
            case "contains":
                return match.Groups["leftValue"].Value.Contains(match.Groups["rightValue"].Value);
            case "does not contain":
                return !match.Groups["leftValue"].Value.Contains(match.Groups["rightValue"].Value);
            case "is in":
                return match.Groups["rightValue"].Value.Contains(match.Groups["leftValue"].Value);
            case "is not in":
                return !match.Groups["rightValue"].Value.Contains(match.Groups["leftValue"].Value);
            case ">":
                if (isLeftValueANumber && isRightValueANumber) return leftValue > rightValue;
                else
                { 
                    Assert.IsTrue(false, "invalide argument " + (isLeftValueANumber ? "right value" : "left value") + "is not a number");
                    return false;
                }
            case ">=":
                if(isLeftValueANumber&&isRightValueANumber) return leftValue>=rightValue;
                else
                {
                    Assert.IsTrue(false, "invalide argument " + (isLeftValueANumber ? "right value" : "left value") + "is not a number");
                    return false;
                }
            case "<":
                if(isLeftValueANumber&&isRightValueANumber) return leftValue<rightValue;
                else
                {
                    Assert.IsTrue(false, "invalide argument " + (isLeftValueANumber ? "right value" : "left value") + "is not a number");
                    return false;
                }
            case "<=":
                if(isLeftValueANumber&&isRightValueANumber) return leftValue<=rightValue;
                else
                {
                    Assert.IsTrue(false, "invalide argument " + (isLeftValueANumber ? "right value" : "left value") + "is not a number");
                    return false;
                }
            case "and":
                if (isLeftValueABool && isRightValueABool) return match.Groups["leftValue"].Value == "true" && match.Groups["rightValue"].Value == "true";
                else
                {
                    Assert.IsTrue(false, "invalide argument " + (isLeftValueABool ? "right value" : "left value") + "is not a boolean");
                    return false;
                }
            case "or":
                if (isLeftValueABool && isRightValueABool) return match.Groups["leftValue"].Value == "true" || match.Groups["rightValue"].Value == "true";
                else
                {
                    Assert.IsTrue(false, "invalide argument " + (isLeftValueABool ? "right value" : "left value") + "is not a boolean");
                    return false;
                }
        }

        return false;
    }

    private string ReplaceVariablesByValues(string text, string delimiter)
    {
        Regex RGX_FindVariable = new Regex(@"\$\D[^\s,]*");

        Match match = RGX_FindVariable.Match(text);
        while (match.Success) 
        {
            text = text.Replace(match.Value, delimiter + parentStrory.GetVariable(match.Value.Substring(1)) + delimiter);
            match = RGX_FindVariable.Match(text);
        }
        return text;
    }

    private void PlaySound(string sound)
    {
        Regex patternSound = new Regex(@"(sound): (?<soundType>.*)");
        Match soundMatch = patternSound.Match(sound);

        string soundType = soundMatch.Groups["type"].Value;
        // Debug.Log(soundType);
        //Assert.IsTrue(false, "Not implemented");
    }

    // ########## GETTER / SETTER ########## 
    public string GetTitle() { return title; }

    public List<NextNode> GetNextNodes() { return nextNodes; }

    public override int GetHashCode() { return HashCode.Combine(title, tags, position, size); }

    public bool HasTag(string tag) { return tags.Contains(tag); }

    public Sprite GetSprite() { return sprite; }

}
