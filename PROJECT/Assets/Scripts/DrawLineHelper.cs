using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(WarNexus))]
public class DrawLineHelper : Editor
{
    static WarNexus[] warNexuses = null;
    static List<WarWay> warWays = new List<WarWay>();

    void OnSceneGUI()
    {
        DrawWarWaysMainLines();
        DrawContainLines();
    }

    private void DrawWarWaysMainLines()
    {
        if (warNexuses == null || warWays == null)
            return;

        for (int i = 0; i < warWays.Count; i++)
        {
            Handles.DrawLine(warWays[i].linkedNexuses[0].transform.position, warWays[i].linkedNexuses[1].transform.position);
        }
    }

    private void DrawContainLines()
    {
        if (warNexuses == null || warWays == null)
            return;

        foreach (var way in warWays)
        {
            Color defoult = Handles.color;
                Handles.color = Color.red;
            for (int i = 0; i < way.finish.Length; i++)
            {
                Handles.DrawLine(way.start[i], way.finish[i]);
                Handles.color = defoult;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WarNexus myScript = (WarNexus)target;
        if (GUILayout.Button("Update road"))
        {
            warWays.Clear();
            warNexuses = FindObjectsOfType<WarNexus>();
            for (int i = 0; i < warNexuses.Length; i++)
            {
                if(warNexuses[i].linkedWarNexus != null)
                {
                    warWays.Add(warNexuses[i].Link(warNexuses[i].linkedWarNexus));
                }
            }

            foreach (WarWay way in warWays)
            {
                way.SetupLines();
            }
        }
        if (GUILayout.Button("Add point"))
        {
            myScript.AddPoint();
        }
    }
}
#else
public class DrawLineHelper : MonoBehaviour
{

}
#endif