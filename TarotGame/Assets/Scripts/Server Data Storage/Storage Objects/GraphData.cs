using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class GraphData : IData
{
    [SerializeField]
    private float[] values;

    /// <summary>
    /// Number of entries in values
    /// </summary>
    public int Count
    {
        get { return values.Length; }
    }

    // Indexer works as an overload for []
    public float this[int i]
    {
        get { return values[i]; }
        set { values[i] = value; }
    }

    public GraphData(float[] values)
    {
        this.values = values;
    }

    public float Average()
    {
        float total = 0;

        foreach (float f in values)
            total += f;

        total /= values.Length;

        return total;
    }

    public override string ToString()
    {
        string str = "(GraphData)[values: [ ";
        foreach(float value in values) 
            str += value.ToString() + (value != values[^1] ? ", " : ""); // if value is the last element then dont add the ", "
        str += " ] ]";

        return str;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GraphData))]
    public class GraphDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("values"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
