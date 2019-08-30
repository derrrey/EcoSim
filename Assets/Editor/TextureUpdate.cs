using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureData))]
public class TextureUpdate : Editor
{
    public override void OnInspectorGUI()
    {
        TextureData textureData = (TextureData)target;

        if (DrawDefaultInspector())
        {
            if (textureData.autoUpdate)
            {
                textureData.NotifyOfUpdatedValues();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            textureData.NotifyOfUpdatedValues();
        }
    }
}
