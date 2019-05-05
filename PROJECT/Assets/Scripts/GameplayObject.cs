using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayObject: MonoBehaviour
{
    public WarWay ownedWarWay = null;
    public virtual void InitObject()
    {

    }
    public virtual void OnClick() { }
    public virtual void OnCamMove() { }
}
