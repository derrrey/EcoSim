using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(WorldManager))]
public class MapGenerate : Editor
{
    public override void OnInspectorGUI()
    {
        WorldManager worldManager = (WorldManager)target;

        if(DrawDefaultInspector())
        {
            if(worldManager.autoUpdate)
            {
                worldManager.GenerateAndDisplayMap();
            }
        }

        if(GUILayout.Button("Generate"))
        {
            worldManager.GenerateAndDisplayMap();
        }
    }
}
