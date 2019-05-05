using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI
{
    private Pawn m_pawn;
    private const float m_checkRadius = 1.0f;

    public virtual void Init(Pawn pawn)
    {
        m_pawn = pawn;
    }

    public virtual Collider GetTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(m_pawn.transform.position, m_checkRadius);
        if (colliders.Length == 0)
            return null;
        else if (colliders.Length == 1)
            return colliders[0];

        float minLen = float.MaxValue;
        Collider curCollider = null;
        foreach (var col in colliders)
        {
            float dist = Vector3.Distance(m_pawn.transform.position, col.transform.position);
            if (dist < minLen)
            {
                minLen = dist;
                curCollider = col;
            }
        }
        return curCollider;
    }
}
