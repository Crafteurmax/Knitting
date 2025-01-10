# Knitting

## Description

Knitting is a simply to use dataparsing package to convert a Twine project into a usable unity gameObject. It use the Harlowe story format wich mean that you can use macros, hooks, variable and all of this good stuff. I'm not sure but I think that it will also parse the Entweedle format.

## Limitation

This package is still in developpement so there is some limitations :

- Variable can only contain string value, if a macro has an argument that is an other type, it will be converted.
- No algebra
- No parentheses or bracket inside variable

## How to use

### Intall in unity

If you want a detail explaination you can read the [official Unity documentation](https://docs.unity3d.com/2020.1/Documentation/Manual/upm-ui-giturl.html), here is a TLDR :

- Copy the git the git URL
- Go to Window > Package Manager
- On the top left corner + > Add package from git URL...
- Paste the link and click on add

If you don't see any error message, it's done !!!!

### Export your story from Twine

Your story need be build in the Harlowe (the default format). From what I understand, Entweedle use the same syntaxe whitout any logic sugar on top of it so I think it can also work, but use it at your own risk.

How to export :

- Open your story
- Go to Build > Export As Twee
- Save the file and change his extension from .twee to .txt
- Drag and drop your file in your unity project

### How to use your story in Unity

In order to use your story in unity you will have to create a story manager, don't be afraid, it's a realy easy and straight forward thing to do !!! You can place your story manager where ever you want in your hierarchy and even have multiple of them !!!

- Right clic on your hierarchy and select Knitting>StoryManager
- Select this newly created StoryManager
- In the component "Story", use the "Twine Text" field to select your story file

Your story is now ready to be use !!! To use it in your scripts you just need to have a reference to this story component in your code.

### Unity code documentation

#### Class : Story

##### GetVariable

Description :
Return the value of a story variable, if this variable is not set it will raise the error "[variableName] is not defined"

```C#
 string GetVariable(string variableName) 
```

Arguments:

- string variableName : the name of the variable without the $ prefix.

Return:

- string : the value of the variable.

Exemple :

```C#
string variableName = "variableName"
string value = story.GetVariable(variableName);
Debug.Log("the variable " + variableName + " as the value \"" + value + "\"")
// will print : the variable variableName as the value [what's in the variable]"
```

##### SetVariable

Description :
Set the value of a story variable, if this variable is not defined yet, it will define and set it.

```C#
 void SetVariable(string variableName, string value) 
```

Arguments:

- string variableName : the name of the variable without the $ prefix.
- string value : the value you want the variable to be

Exemple :

```C#
string playerName = "Bob";
story.SetVariable("playerName", playerName);

Debug.log("the name of the player is " + story.GetVariable("playerName"));
```

##### NextNode

Description :
Will change the current node to the default next node (aka, the first link in your node text). Very usefull whe you only have one only option for your next node, if you have multiple of them please use the [ChooseNextNode](##### ChooseNextNode) methode.

```C#
void NextNode()
 
```

##### ChooseNextNode

Description :
Will change the current node to the choosen destination (the top one beeing at the index 0). Very usefull whe you only have multiple options for your nexts nodes, if you have only one of them please use the [NextNode](##### NextNode) methode.

```C#
 void ChooseNextNode(int index)
```

Arguments:

- int index : the index of the choosen next node

##### GetCurrentNode

Description :
Return the current node.

```C#
 StoryNode GetCurrentNode()
```

Return:

- StoryNode : the current node, the very first beeing the node set as the start node in Twine

Exemple :

```C#
StoryNode currentNode = story.GetCurrentNode();
Debug.Log(currentNode.GetText());
story.NextNode();
```

##### All of the other Getters

Description :
get the value of the Twine defined values like Title, Ifid, Format, FormatVersion, Zoom, Start, Tag colors. Excepte GetZoom() and GetTagColors() that will repectively return an int and a Dictionary<string, Color>, they all return a string

#### Class StoryNode

##### StoryNode (constructor)

Description :
Used to generate each node, please don't use it in your code. How ever, because of it's public and I can't let only one public methode not be in this documentations, here it is. Maybe it could be usefull for something beyond my undrestanding.

```C#
StoryNode(string _title, string _data, Story _parentStrory)
StoryNode(string _title, List<string> _tags, Vector2Int _position, Vector2Int _size, string _data, Story _parentStrory)
```

Arguments:

- string _title : the title of the node in your twine, this need to be unique.
- string _data : the text, including macro and links to other nodes.
- Story _parentStrory : the story that own this node.
- List\<string> _tags : : all of the tags of the node.
- Vector2Int _position : the position of this node in twine.
- Vector2Int _size : the size of the node in twine.

Return:

- StoryNode : the node that you've just created.

##### ToString

Description :
an override of the basic function that will return the title of the node.

```C#
string ToString()
```

##### Equals

Description :
an override of the basic function that will return true if nodes' title, position, size and tags are the same.

```C#
bool Equals(object obj)
```

Arguments:

- object obj : any objects

Return:

- bool : if the two objects are equals

##### getText

Description :
return the node's text and execute all macros that it contains

```C#
string getText()
```

Return:

- string : the text will all macro executed and variable replaced

Exemple :

```C#
/*
for a node that containt as a raw text :
"
(set: $something to 'foo')
Tell me clever machine. What's the best way to achieve my ambitions?
"
*/

Debug.Log(node.GetText());

// will print "Tell me clever machine. What's the best way to achieve my ambitions?" and set the variable "something" to "foo"
```

##### GetTitle

Description :
return the title of the node.

```C#
string GetTitle()
```

##### GetNextNodes

Description :
Get the list of all the nodes that this node links to.

```C#
List<NextNode> GetNextNodes()
```

Return:

- List\<NextNode> : the list of all of following node with title and display name.

##### HasTag

Description :
return true if the node has the tag you ask for.

```C#
bool HasTag(string tag)
```

Arguments:

- string tag : the name of the tag.

Return:

- bool : true if the node has the tag

Exemple :

```C#
if(node.HasTag("Bob") Debug.Log("Bob : " + node.GetText()))
else Debug.Log("Someone else : " + node.GetText())
```

## Known issue
