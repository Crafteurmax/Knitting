using UnityEngine;
using UnityEditor;

public static class MenuItems
{
    [MenuItem("GameObject/Knitting/StoryManager")]
    static void CreateStory(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("StoryManager");
        go.AddComponent<Story>();

        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
