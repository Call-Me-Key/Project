using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public WarWay m_targetWay;
    public Vector3 m_camAngle = new Vector3(0,45,0);
    [SerializeField]
    private float m_camFar = 5;
    private Vector3 m_camTarget = new Vector3();
    public Vector3 camTarget { get { return m_camTarget; } }
    private Vector3 m_moveVector = new Vector3();
    private Vector3 m_camNormalFromWay = new Vector3();
    private float m_wayLen = 0;
    public Camera assignedCamera;

    private Wall m_currentTransparencyWall = null;

    // Start is called before the first frame update
    void Start()
    {
        assignedCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.D))
        {
            m_camTarget = m_camTarget + Vector3.Project(transform.right, m_moveVector).normalized;
            ResetLook();
        }
        if(Input.GetKey(KeyCode.A))
        {
            m_camTarget = m_camTarget + Vector3.Project(-transform.right, m_moveVector).normalized;
            ResetLook();
        }
        if ((Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0) && Camera.current != null)
        {
            
            Ray camRay = Camera.current.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            var col = Physics.Raycast(camRay, out hit, 100);
            if(col)
                SetObjectTransparancy(hit.collider.gameObject);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.current == null)
                return;
            Ray camRay = Camera.current.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            var col = Physics.Raycast(camRay, out hit, 1000);
            GameplayObject obj = hit.collider.gameObject.GetComponent<GameplayObject>();
            obj.OnClick();

            //var col = Physics.RaycastAll(camRay, 1000);
        }
    }

    public void SetObjectTransparancy(GameObject obj)
    {
        Wall wall = obj.GetComponent<Wall>();
        if (wall != null)
        {
            wall.SetTransparancy(true);
            if (m_currentTransparencyWall && wall != m_currentTransparencyWall)
                m_currentTransparencyWall.SetTransparancy(false);
        }
        m_currentTransparencyWall = wall;
    }

    public void ResetLook()
    {
        Vector3 campos = m_camNormalFromWay * m_camFar;
        campos = Quaternion.Euler(m_camAngle) * campos;
        transform.position = campos + m_camTarget;
        transform.LookAt(m_camTarget);
        EventManager.cameraMove.Invoke(this);
    }

    public void SetWay(WarWay way, Vector3 start)
    {
        m_targetWay = way;
        m_wayLen = (way.start[0] - way.finish[0]).magnitude;
        m_moveVector = (way.start[0] - way.finish[0]).normalized;
        m_camNormalFromWay = way.start[0] - way.start[1];
        m_camTarget = start;
        ResetLook();
    }
}
