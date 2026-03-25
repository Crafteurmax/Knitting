using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Runtime.ANTLR.Value
{
    public class Value : Expression.Expression, IComparable<Value>
    {
        public ValueType valueType;
        
        public string stringValue;
        public int intValue;
        public float floatValue;
        public bool boolValue;
        public List<Value> arrayValue = new List<Value>();

        public Value(string value)
        {
            valueType = ValueType.String;
            this.stringValue = value;
        }
        public Value(int value)
        {
            valueType = ValueType.Integer;
            this.intValue = value;
        }
        public Value(float value)
        {
            valueType = ValueType.Floating;
            this.floatValue = value;
        }
        public Value(bool value)
        {
            valueType = ValueType.Boolean;
            this.boolValue = value;
        }
        public Value(List<Value> value)
        {
            valueType = ValueType.Array;
            this.arrayValue = value;
        }

        public static implicit operator string(Value v)
        {
            switch (v.valueType)
            {
                case ValueType.String: return v.stringValue;
                case ValueType.Integer: return v.intValue.ToString();
                case ValueType.Floating: return v.floatValue.ToString(CultureInfo.InvariantCulture);
                case ValueType.Boolean: return v.boolValue.ToString();
                case ValueType.Array: throw new NotImplementedException(); // TODO
            }
            
            throw new NotImplementedException(); // TODO
        }
        public static implicit operator int(Value v)  
        {  
            switch (v.valueType)
            {
                case ValueType.String: 
                    int.TryParse(v.stringValue, out int value);
                    return value;
                case ValueType.Integer: return v.intValue;
                case ValueType.Floating: return (int) v.floatValue;
                case ValueType.Boolean: return v.boolValue ?  1 : 0;
                case ValueType.Array: throw new NotImplementedException(); // TODO
            }  
            
            throw new NotImplementedException(); // TODO
        }
        public static implicit operator float(Value v)  
        {  
            switch (v.valueType)
            {
                case ValueType.String: 
                    float.TryParse(v.stringValue, out float value);
                    return value;
                case ValueType.Integer: return v.intValue;
                case ValueType.Floating: return v.floatValue;
                case ValueType.Boolean: return v.boolValue ?  1f : 0f;
                case ValueType.Array: throw new NotImplementedException(); // TODO
            }  
            
            throw new NotImplementedException(); // TODO
        }

        public static implicit operator bool(Value v)
        {
            switch (v.valueType)
            {
                case ValueType.Boolean: return v.boolValue;
                case ValueType.String: 
                    if(v.stringValue == "true") return true;
                    if(v.stringValue == "false") return false;
                    break;
            }  
            
            throw new NotImplementedException(); // TODO
            
        }

        public static implicit operator List<Value>(Value v)
        {
            
            switch (v.valueType)
            {
                case ValueType.Array: return v.arrayValue;
            }  
            
            throw new NotImplementedException(); // TODO  
            
        }


        public int CompareTo(Value other)
        {
            if(valueType != other.valueType) throw new NotImplementedException(); // TODO

            switch (valueType)
            {
                case ValueType.String: return stringValue.CompareTo(other.stringValue);
                case ValueType.Integer: return intValue.CompareTo(other.intValue);
                case ValueType.Floating: return floatValue.CompareTo(other.floatValue);
                case ValueType.Boolean: return boolValue.CompareTo(other.boolValue);
                case ValueType.Array: throw new NotImplementedException(); // TODO
            }
            
            throw new NotImplementedException(); // TODO
        }

        public override string ToString()
        {
            // Debug.Log("ToString");
            switch (valueType)
            {
                case ValueType.String:
                    return "\"" + stringValue + "\"" ;
                case ValueType.Integer:
                    return intValue.ToString();
                case ValueType.Floating:
                    return floatValue.ToString(CultureInfo.InvariantCulture);
                case ValueType.Boolean:
                    return boolValue.ToString();
                case ValueType.Array:
                    string toReturn = "{";
                    for (int i = 0; i < arrayValue.Count; i++)
                    {
                        toReturn += arrayValue[i].ToString() + ",";
                    }
                    toReturn += "}";
                    return toReturn;;
            }
            
            return "\\# error in value toString #/";
        }

        public override string Collapse(Story story, StoryNode storyNode)
        {
            // Debug.Log("Collapse");
            switch (valueType)
            {
                case ValueType.String:
                    return stringValue;
                case ValueType.Integer:
                    return intValue.ToString();
                case ValueType.Floating:
                    return floatValue.ToString(CultureInfo.InvariantCulture);
                case ValueType.Boolean:
                    return boolValue.ToString();
                case ValueType.Array:
                    string toReturn = "{";
                    for (int i = 0; i < arrayValue.Count; i++)
                    {
                        toReturn += arrayValue[i].ToString() + ",";
                    }
                    toReturn += "}";
                    return toReturn;;
            }
            
            return "\\# error in value toString #/";
        }

        public override Value Evaluate(Story story, StoryNode storyNode)
        {
            // Debug.Log("Evaluate");
            return this;
        }
    }

    public enum ValueType
    {
        String,
        Integer,
        Floating,
        Boolean,
        Array,
        Variable
    }
}
