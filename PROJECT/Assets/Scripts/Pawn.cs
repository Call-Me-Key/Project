using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pawn : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private Vector3[] m_wayOn = new Vector3[2];
    private Vector3 m_force;

    public bool debug_enemy;
    // Start is called before the first frame update
    void Start()
    {
        FitRigidbody();
    }
    public void FitRigidbody()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.DrawLine(transform.position, transform.position + m_force);
        // m_rigidbody.velocity = Vector3.Project(m_rigidbody.velocity, m_force);
        //m_rigidbody.velocity = m_rigidbody.velocity + m_force;
    }

    private void LateUpdate()
    {
        //m_rigidbody.velocity = m_force;// Vector3.Project(m_rigidbody.velocity, m_force);
        FixPosition();
    }

    public void SetWay(EnterPoint enterPoint)
    {
        EnterPointStruct entrestr = enterPoint.GetEnterPosition();
        m_wayOn[0] = entrestr.warWay.start[entrestr.pos.line];
        m_wayOn[1] = entrestr.warWay.finish[entrestr.pos.line];
        //m_force = (m_wayOn[0] - m_wayOn[0]).normalized;
        transform.LookAt(m_wayOn[1]);
        Vector3 forward = (m_wayOn[1] - m_wayOn[0]).normalized;

        transform.position = entrestr.warWay.start[entrestr.pos.line] + (forward * entrestr.pos.lenFromStart);
    }

    private void FixPosition()
    {
        //const float maxLen = 0.1f;
        //Vector3 mainVec = m_wayOn[0] - m_wayOn[1];
        //Vector3 heroVec = m_wayOn[0] - transform.position;
        //Vector3 projection = Vector3.Project(heroVec, mainVec);
        //if(Vector3.Distance(heroVec, projection) > maxLen)
        //{
        //    transform.position = m_wayOn[0] + projection;
        //}
    }

}
