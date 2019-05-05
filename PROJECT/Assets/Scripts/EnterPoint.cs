using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameplayObject))]
public class EnterPoint : MonoBehaviour
{
    public GameplayObject gameplayObject { get; private set; }
    public List<OnWayPosition> nearPositions = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameplayObject == null)
            return;

        WarWay ww = gameplayObject.ownedWarWay;

        foreach (var point in nearPositions)
        {
            Vector3 forward = (ww.finish[0] - ww.start[0]).normalized;
            Vector3 p = ww.start[point.line] + (forward * point.lenFromStart);
            Debug.DrawRay(p, Vector3.up * 1000, Color.white);
        }
    }

    public EnterPointStruct GetEnterPosition()
    {
        EnterPointStruct enterPointStruct = new EnterPointStruct();
        enterPointStruct.pos = nearPositions[0];
        enterPointStruct.warWay = gameplayObject.ownedWarWay;
        return enterPointStruct;
    }

    public void ResetNearPoints()
    {
        if(gameplayObject == null)
            gameplayObject = GetComponent<GameplayObject>();

        nearPositions = gameplayObject.ownedWarWay.GetAllClosestOnWayPositions(transform.position);
    }

}

public struct EnterPointStruct
{
    public OnWayPosition pos;
    public WarWay warWay;
}