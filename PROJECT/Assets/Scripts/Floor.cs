using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Floor : GameplayObject
{
    public NavMeshSurface navSurface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InitObject()
    {
        base.InitObject();
        navSurface = GetComponent<NavMeshSurface>();
    }
}
