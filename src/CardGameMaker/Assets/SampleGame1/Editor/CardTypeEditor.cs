#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardType))]
public class CardTypeEditor : Editor
{
    private SerializedProperty id, cardName, art, description, numCopies;

    public void OnEnable()
    {
        cardName = serializedObject.FindProperty("cardName");
        art = serializedObject.FindProperty("art");
        description = serializedObject.FindProperty("description");
        numCopies = serializedObject.FindProperty("numCopies");
    }

    public override void OnInspectorGUI()
    {
        PresentUnchanged(art);
        PresentUnchanged(cardName);
        PresentUnchanged(numCopies);
        PresentUnchanged(description);
    }

    private void PresentUnchanged(SerializedProperty serializedProperty)
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedProperty, includeChildren: true);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif
