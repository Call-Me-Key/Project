using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Anima2D;


[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class Pawn : MonoBehaviour
{
    public List<SkinnedMeshCombiner> combiners = new List<SkinnedMeshCombiner>();
    private Rigidbody m_rigidbody;
    private Vector3[] m_wayOn = new Vector3[2];
    private Vector3 m_forwardWayVector;
    private NavMeshAgent m_navAgent;

    [SerializeField]
    private int m_health = 100;
    public DamageTypeStruct armor = new DamageTypeStruct();
    public DamageTypeStruct damage = new DamageTypeStruct();

    BaseAI m_ai = new BaseAI();

    public bool debug_enemy;
    // Start is called before the first frame update
    void Start()
    {
        InitObject();
        DamageFlash();
        m_ai.Init(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FixedUpdate()
    {
        if(m_navAgent.velocity.magnitude > 0)
        {
            if(Vector3.Dot(m_navAgent.velocity, m_forwardWayVector) < 0)
            {
                transform.rotation = Quaternion.LookRotation(m_forwardWayVector);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(-m_forwardWayVector);
            }
        }
    }

    public virtual void InitObject()
    {
        m_navAgent = GetComponent<NavMeshAgent>();

        m_forwardWayVector = m_wayOn[1] - m_wayOn[0];

        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void SetWay(EnterPoint enterPoint)
    {
        EnterPointStruct entrestr = enterPoint.GetEnterPosition();
        m_wayOn[0] = entrestr.warWay.start[entrestr.pos.line];
        m_wayOn[1] = entrestr.warWay.finish[entrestr.pos.line];
        transform.LookAt(m_wayOn[1]);
        Vector3 forward = (m_wayOn[1] - m_wayOn[0]).normalized;

        transform.position = entrestr.warWay.start[entrestr.pos.line] + (forward * entrestr.pos.lenFromStart);
    }

    protected void DamageFlash()
    {
        StartCoroutine(CoroutineDamageFlash());
    }

    protected IEnumerator CoroutineDamageFlash()
    {
        float time = 0.5f;
        //float halftime = time / 2.0f;
        float timeIt = 0;

        while(timeIt < time)
        {
            //var col = Color.Lerp(Color.white, Color.red, Mathf.PingPong(timeIt, halftime) / halftime);
            var col = Color.Lerp(Color.red, Color.white, timeIt / time);
            foreach (var comb in combiners)
            {
                comb.materialPropertyBlock.SetColor("_ColorModif", col);
            }
            timeIt += Time.deltaTime;
            yield return null;
        }
    }

    public int GetDamage(DamageTypeStruct damage)
    {
        int d = (armor - damage).Normalized();
        m_health -= d;
        if(m_health <= 0)
            Die();
        return d;
    }

    public virtual void Die()
    {
        DelayedDestroy(5);
    }

    public IEnumerator DelayedDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this);
    }

    public virtual void Attack(Pawn pawn)
    {
        pawn.GetDamage(damage);
    }
}

