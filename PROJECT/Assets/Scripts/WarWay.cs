using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarWay
{
    private static float m_lineSpaceWidth = 5;
    public const int warWayLineCount = 5;
    public Vector3[] start = new Vector3[warWayLineCount];
    public Vector3[] finish = new Vector3[warWayLineCount];
    public WarNexus[] linkedNexuses = new WarNexus[2];
    public bool swap = false;
    public List<EnvironmentObject> environmentObjects = new List<EnvironmentObject>();

    public List<Wall> leftSideWallsPrefabs = new List<Wall>();
    public List<Wall> rightSideWallsPrefabs = new List<Wall>();

    public List<GameObjectListWrapper> floorPrefabList = new List<GameObjectListWrapper>();
    public void LinkNexuses(WarNexus first, WarNexus second)
    {
        linkedNexuses[0] = first;
        linkedNexuses[1] = second;
        first.warWay = this;
        second.warWay = this;
    }

    public void SetupLines()
    {
        Vector3 normalSideVector = linkedNexuses[1].transform.position - linkedNexuses[0].transform.position;
        normalSideVector.y = 0;
        normalSideVector = Quaternion.Euler(0, 90, 0) * normalSideVector; // rotate it
        normalSideVector = normalSideVector.normalized;
        int offset = -warWayLineCount / 2;
        for (int i = 0; i < warWayLineCount; i++)
        {
            int step = offset + i;
            Vector3 point = normalSideVector * step;
            start[i] = point + linkedNexuses[0].transform.position;
            finish[i] = point + linkedNexuses[1].transform.position;
        }

        if (swap)
        {
            Vector3[] tempStart = new Vector3[warWayLineCount];
            Vector3[] tempFinish = new Vector3[warWayLineCount];

            start.CopyTo(tempStart, 0);
            finish.CopyTo(tempFinish, 0);
            for (int i = 0; i < warWayLineCount; i++)
            {
                start[i] = tempStart[warWayLineCount - i - 1];
                finish[i] = tempFinish[warWayLineCount - i - 1];
            }
        }
    }

    public void RotateEnvironment()
    {
        Vector3 norm = start[0] - finish[0];
        foreach (var env in environmentObjects)
        {
            Vector3 vec = env.transform.position - finish[0];
            Vector3.Project(vec, norm);
            env.transform.LookAt(vec + finish[0]);
        }
    }

    public void SetupEnvironment()
    {
        Ray middleRay = new Ray(
            linkedNexuses[0].transform.position,
            linkedNexuses[1].transform.position - linkedNexuses[0].transform.position
            );
        Ray targetRay = new Ray(
            start[0],
            start[1] - start[0]
            );
        foreach (var env in environmentObjects)
        {
            targetRay.origin = middleRay.GetPoint(env.position);
            env.transform.position = targetRay.GetPoint(env.farPosition);
        }
    }

    public void MakeEndWalls(GameObject wallPrefab, bool isStartSideWall)
    {
        GameObject newWall = GameObject.Instantiate(wallPrefab);
        Vector3 scale = newWall.transform.localScale;
        scale.x = warWayLineCount;
        newWall.transform.localScale = scale;
        Vector3 forwardVector = (finish[0] - start[0]).normalized;
        if (isStartSideWall)
        {
            Vector3 middle = (start[warWayLineCount - 1] - start[0]) / 2;
            middle = start[0] + middle;
            middle += (-forwardVector * (newWall.transform.localScale.y / 2));
            newWall.transform.position = middle + (newWall.transform.up * (newWall.transform.localScale.y / 2));
            newWall.transform.LookAt(newWall.transform.position + forwardVector);
        }
        else
        {
            Vector3 middle = (finish[warWayLineCount - 1] - finish[0]) / 2;
            middle = finish[0] + middle;
            middle += (forwardVector * (newWall.transform.localScale.y / 2));
            newWall.transform.position = middle + (newWall.transform.up * (newWall.transform.localScale.y / 2));
            newWall.transform.LookAt(newWall.transform.position + -forwardVector);
        }
    }

    public void MakeSideWallsFromPrefabList()
    {
        Vector3 lookVector = (start[1] - start[0]).normalized;
        Vector3 forwardVector = (finish[0] - start[0]).normalized;
        Vector3 zeroPos = start[0] - lookVector;
        float pos = 0;
        foreach (var wall in leftSideWallsPrefabs)
        {
            GameObject newWall = GameObject.Instantiate(wall.gameObject);
            newWall.transform.LookAt(newWall.transform.position + lookVector);
            newWall.transform.position = zeroPos
                + (newWall.transform.up * (newWall.transform.localScale.y / 2)) 
                + forwardVector * (pos + newWall.transform.localScale.x / 2);
            pos += newWall.transform.localScale.x;
            var go = newWall.GetComponent<GameplayObject>();
            go.ownedWarWay = this;

            //TODO: Call "ResetnearPoints" via event!!!
            var ep = newWall.GetComponent<EnterPoint>();
            if (ep != null)
            {
                ep.ResetNearPoints();
                Debug.Log("!");
            }
        }
        zeroPos = start[warWayLineCount - 1] + lookVector;
        pos = 0;
        foreach (var wall in rightSideWallsPrefabs)
        {
            GameObject newWall = GameObject.Instantiate(wall.gameObject);
            newWall.transform.LookAt(newWall.transform.position + lookVector);
            newWall.transform.position = zeroPos 
                + (newWall.transform.up * (newWall.transform.localScale.y / 2))
                + forwardVector * (pos + newWall.transform.localScale.x / 2);
            pos += newWall.transform.localScale.x;

            var go = newWall.GetComponent<GameplayObject>();
            go.ownedWarWay = this;

            //TODO: Call "ResetnearPoints" via event!!!
            var ep = newWall.GetComponent<EnterPoint>();
            if(ep != null)
            {
                ep.ResetNearPoints();
            }
        }
    }

    public void MakeFloorFromPrefabList()
    {
        Vector3 forwardVector = (finish[0] - start[0]).normalized;

        for (int i = 0; i < floorPrefabList.Count; i++)
        {
            float pos = 0.0f;
            foreach (var floor in floorPrefabList[i].gameObjects)
            {
                GameObject newFloor = GameObject.Instantiate(floor.gameObject);
                newFloor.transform.LookAt(newFloor.transform.position + forwardVector);
                newFloor.transform.position = start[i] 
                    + (-newFloor.transform.up * (newFloor.transform.localScale.y / 2)) 
                    + forwardVector * (pos + (newFloor.transform.localScale.z / 2));
                pos += newFloor.transform.localScale.z;

                var go = newFloor.GetComponent<GameplayObject>();
                go.ownedWarWay = this;
            }
        }
    }

    public void SetLenAccordingLongestObstructList()
    {
        float maxLen = 0;
        float tmpLen = 0;
        foreach (var wall in leftSideWallsPrefabs)
            tmpLen += wall.transform.localScale.x;
        maxLen = Mathf.Max(maxLen, tmpLen);
        tmpLen = 0;
        foreach (var wall in rightSideWallsPrefabs)
            tmpLen += wall.transform.localScale.x;
        maxLen = Mathf.Max(maxLen, tmpLen);
        tmpLen = 0;
        for (int i = 0; i < floorPrefabList.Count; i++)
        {
            foreach (var floor in floorPrefabList[i].gameObjects)
                tmpLen += floor.transform.localScale.x;
            maxLen = Mathf.Max(maxLen, tmpLen);
            tmpLen = 0;
        }
        Vector3 forwardVector = (linkedNexuses[1].transform.position - linkedNexuses[0].transform.position).normalized;
        linkedNexuses[1].transform.position = linkedNexuses[0].transform.position + maxLen * forwardVector;
        SetupLines();
    }

    public List<OnWayPosition> GetAllClosestOnWayPositions(Vector3 position)
    {
        List<OnWayPosition> onWayPositions = new List<OnWayPosition>();
        Vector3[] tempPos = new Vector3[warWayLineCount];
        float[] tempLen = new float[warWayLineCount];
        for (int i = 0; i < warWayLineCount; i++)
        {
            tempPos[i] = WarWay.ClosestPointOnLine(start[i], finish[i], position);
            tempLen[i] = Vector3.Distance(start[i], tempPos[i]);
            onWayPositions.Add(new OnWayPosition(i, 
                Vector3.Distance(start[i], tempPos[i]), 
                Vector3.Distance(position, tempPos[i])));
        }
        Debug.Log("Before: " + onWayPositions[0].line + " : " + onWayPositions[0].lenFromEnterPoint);

        onWayPositions.Sort((emp1, emp2) => emp1.lenFromEnterPoint.CompareTo(emp2.lenFromEnterPoint));

        Debug.Log("After: " + onWayPositions[0].line + " : " + onWayPositions[0].lenFromEnterPoint);

        //for (int i = 0; i < onWayPositions.Count; i++)
        //{
        //    Debug.Log(onWayPositions[i].line);
        //}



        //float curLen = float.MaxValue;
        //for (int i = 0; i < warWayLineCount; i++)
        //{
        //    for (int q = 0; q < warWayLineCount; q++)
        //    {
        //        if (tempLen[q] < curLen)
        //        {
        //            curLen = tempLen[i];
        //            onWayPositions[i] = new OnWayPosition(q, curLen);
        //        }
        //    }
        //}
        return onWayPositions;
    }

    public OnWayPosition GetClosestOnWayPosition(Vector3 position)
    {
        OnWayPosition onWayPositions = new OnWayPosition();
        Vector3 tempPos;
        onWayPositions.lenFromStart = float.MaxValue;
        for (int i = 0; i < warWayLineCount; i++)
        {
            tempPos = WarWay.ClosestPointOnLine(start[i], finish[i], position);
            float curDist = Vector3.Distance(position, tempPos);
            if (curDist < onWayPositions.lenFromStart)
            {
                onWayPositions.lenFromStart = curDist;
                onWayPositions.line = i;
            }
        }

        return onWayPositions;
    }

    public static Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint)
    {
        Vector3 vVector1 = vPoint - vA;
        Vector3 vVector2 = (vB - vA).normalized;

        float d = Vector3.Distance(vA, vB);
        float t = Vector3.Dot(vVector2, vVector1);

        if (t <= 0)
            return vA;

        if (t >= d)
            return vB;

        Vector3 vVector3 = vVector2 * t;

        return (vA + vVector3);
    }
}

public struct OnWayPosition
{
    public int line;
    public float lenFromStart;
    public float lenFromEnterPoint;

    public OnWayPosition(int newLine, float newLen, float newLenFromEnterPoint)
    {
        line = newLine;
        lenFromStart = newLen;
        lenFromEnterPoint = newLenFromEnterPoint;
    }
}