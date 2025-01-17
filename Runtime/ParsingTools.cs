using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class ParsingTools
{
    public class ParsingResult
    {
        public Dictionary<string, string> Groups = new Dictionary<string, string>();
        public string Value;
        public bool succes = false;

        public int startIndex;
        public int endIndex;

        override public bool Equals(object _other)
        {
            if(_other.GetType() != typeof(ParsingResult)) return false;
            ParsingResult other = (ParsingResult)_other;
            if (this.Value != other.Value) return false;
            if (this.Groups.Count != other.Groups.Count) return false;
            foreach (KeyValuePair<string, string> pair in this.Groups) 
            {
                string value;
                if(!other.Groups.TryGetValue(pair.Key, out value)) return false;
                if (value != pair.Value) return false;
            }
            if (this.succes != other.succes) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Groups, Value, succes);
        }

        override public string ToString() {  return Value; }
    }

    static public int FindInWord(string text, char separator, int offset = 0)
    {
        bool isInGuillemet = false;
        bool isInApostrophe = false;
        bool skipnext = false;

        for (int i = offset; i < text.Length; i++)
        {
            if (!isInApostrophe && !isInGuillemet && text[i] == separator) return i;
            if (!skipnext && !isInGuillemet && text[i] == '\'') isInApostrophe = !isInApostrophe;
            if (!skipnext && !isInApostrophe && text[i] == '"') isInGuillemet = !isInGuillemet;
        }

        return -1;
    }

    static public List<string> CarefullSplit(string text, char separator, int offset = 0)
    {
        List<string> splited = new List<string>();

        int index = FindInWord(text, separator, offset);
        while (index != -1)
        {
            splited.Add(text.Substring(offset,index - offset));
            offset = index + 1;
            index = FindInWord(text, separator, offset);
        }

        if (offset < text.Length) splited.Add(text.Substring(offset, text.Length - offset));
        else if (offset == text.Length) splited.Add("");

        return splited;
    }

    static public ParsingResult GetBetween(string text, char begin, char end, int offset = 0) 
    {
        int startIndex = FindInWord(text, begin, offset);
        int endIndex = FindInWord(text, end, startIndex + 1);

        ParsingResult result = new ParsingResult();
        result.succes = false;

        if (startIndex < 0 || endIndex < 0)
        {
            return result;
        }

        result.Value = text.Substring(startIndex , endIndex - startIndex + 1);
        result.startIndex = startIndex;
        result.endIndex = endIndex + 1;
        result.succes = true;
        result.Groups.Add("Inside",text.Substring(startIndex + 1, endIndex - startIndex - 1));

        return result;
    }

    
    static public ParsingResult GetCommande(string text, int offset = 0)
    {
        ParsingResult result = new ParsingResult();

        ParsingResult macro = GetBetween(text, '(', ')', offset);
        if (!macro.succes) return macro;
        result.startIndex = macro.startIndex;
        result.endIndex = macro.endIndex;
        result.Groups.Add("commande", macro.Groups["Inside"]);
        result.succes = true;

        if (macro.endIndex + 1 < text.Length && text[macro.endIndex] == '[')
        {
            ParsingResult hook = GetBetween(text, '[', ']', macro.endIndex);

            if (hook.succes)
            {
                result.Groups.Add("text", hook.Groups["Inside"]);
                result.endIndex = hook.endIndex;
            }
        }

        result.Value = text.Substring(result.startIndex, result.endIndex - result.startIndex);

        return result ;
    }

    static public List<ParsingResult> GetAllCommandes(string text, int offset = 0)
    {
        List<ParsingResult> result = new List<ParsingResult>();

        ParsingResult match = GetCommande(text, offset);

        while (match.succes) 
        {
            result.Add(match);
            match = GetCommande(text,match.endIndex + 1);
        }

        return result ;

    }

    static public List<string> GetOpposit(string text, List<ParsingResult> complement)
    {
        List<string> result = new List<string>();

        int start = 0;

        foreach (ParsingResult complementItem in complement)
        {
            result.Add(text.Substring(start,complementItem.startIndex - start ));
            start = complementItem.endIndex;
        }

        if (start < text.Length) result.Add(text.Substring(start, text.Length - start));

        return result ;
    }

    static public ParsingResult GetSet(string text, int offset = 0) 
    {
        ParsingResult result = new ParsingResult();

        int variableNameStart = -1;
        int variableNameEnd = -1;

        for (int i = offset; i < text.Length; i++) 
        {
            if (text[i] == '$' && variableNameStart == -1) variableNameStart = i + 1;
            else if (text[i] == ' ' && variableNameStart != -1 && variableNameEnd == -1) variableNameEnd = i;
            else if (variableNameEnd != -1) break;
        }

        if (variableNameStart == -1 || variableNameEnd == -1) return result ;

        result.Groups.Add("variableName", text.Substring(variableNameStart, variableNameEnd - variableNameStart));

        ParsingResult value = GetWord(text,variableNameEnd);
        if (!value.succes) return result ;

        result.Groups.Add("value", value.Groups["Inside"]);

        result.Value = text;
        result.succes = true ;
        result.startIndex = 0;
        result.endIndex = text.Length;

        return result;
    }

    static public ParsingResult GetWord(string text, int offset = 0)
    {
        ParsingResult valueGuillemet = GetBetween(text, '"', '"', offset);
        ParsingResult valueApostrophe = GetBetween(text, '\'', '\'', offset);

        return valueGuillemet.succes ? valueGuillemet : valueApostrophe;
    }


    static public List<ParsingResult> GetAllWords(string text, int offset = 0)
    {
        List<ParsingResult> result = new List<ParsingResult>();

        ParsingResult match = GetWord(text, offset);

        while (match.succes)
        {
            result.Add(match);
            match = GetWord(text, match.endIndex + 1);
        }

        return result;

    }

}
