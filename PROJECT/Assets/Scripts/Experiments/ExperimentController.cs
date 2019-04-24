using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentController : MonoBehaviour
{
    public WarNexus nexus;
    public Transform explodePosition;
    public CameraController controller;
    private Vector3 explodePoint = Vector3.zero;
    // Start is called before the first frame update

    void Start()
    {
        float len = (nexus.linkedWarNexus.transform.position - nexus.transform.position).magnitude;
        for (int i = 0; i < 20; i++)
        {
            AddEnvObj(Random.RandomRange(0, 20), Random.RandomRange(0, len));
        }
        nexus.warWay.SetupEnvironment();

        GameObject wallPrefab = (GameObject)Resources.Load("SimpleWall");
        nexus.warWay.SetLenAccordingLongestObstructList();
        nexus.warWay.MakeEndWalls(wallPrefab, true);
        nexus.warWay.MakeEndWalls(wallPrefab, false);
        nexus.warWay.MakeSideWallsFromPrefabList();
        nexus.warWay.MakeFloorFromPrefabList();
    }

    private void AddEnemy(EnterPoint enterPoint)
    {
            GameObject variableForPrefab = (GameObject)Resources.Load("SimplePawn");
            variableForPrefab = Instantiate(variableForPrefab);
            Pawn pawn = variableForPrefab.GetComponent<Pawn>();
            pawn.FitRigidbody();
            pawn.SetWay(enterPoint);
    }

    private void AddEnvObj(float far, float pos)
    {
        GameObject variableForPrefab = (GameObject)Resources.Load("SimpleEnvObj");
        variableForPrefab = Instantiate(variableForPrefab);
        EnvironmentObject env = variableForPrefab.GetComponent<EnvironmentObject>();
        nexus.warWay.environmentObjects.Add(env);
        env.position = pos;
        env.farPosition = far;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(nexus.warWay.start[0], nexus.warWay.finish[0]);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var ePoints = FindObjectsOfType<EnterPoint>();
            int it = (int)Random.Range(0, ePoints.Length);
            AddEnemy(ePoints[it]);
        }
        Debug.DrawRay(explodePoint, Vector3.up * 100000);
        if (Input.GetKey(KeyCode.E))
        {
            controller.SetWay(nexus.warWay, nexus.transform.position);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            explodePoint = explodePosition.position;
            float explodeRadius = 5;
            float explodeForce = 1000;
            Collider[] objects = UnityEngine.Physics.OverlapSphere(explodePoint, explodeRadius);
            foreach (Collider h in objects)
            {
                Rigidbody r = h.GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.AddExplosionForce(explodeForce, explodePoint, explodeRadius);
                }
            }
        }
    }
}
