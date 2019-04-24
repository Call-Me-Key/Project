using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : GameplayObject
{
    private Color m_standartColor;
    private Renderer m_renderer;
    // Start is called before the first frame update
    void Start()
    {
        m_standartColor = gameObject.GetComponent<Renderer>().material.color;
        m_renderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetAxis("Mouse X") > 0 || (Input.GetAxis("Mouse Y") > 0))
        //{
        //    SetTransparancy();
        //}
    }

    public void SetTransparancy(bool transparancy)
    {
        if(transparancy)
        {
            Color color = m_renderer.material.color;

            color.a = 0.5f;
            m_renderer.material.color = color;
        }
        else
        {
            m_renderer.material.color = m_standartColor;
        }
           
    }
    public void ClickCaller()
    {
        throw new System.NotImplementedException();
    }

    public void OnCamMove()
    {
        throw new System.NotImplementedException();
    }


}
