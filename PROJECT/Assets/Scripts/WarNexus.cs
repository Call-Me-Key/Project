using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class WarNexus : MonoBehaviour
{
    // an array of game objects which will have a
    // line drawn to in the Scene editor
    public WarWay warWay = null;
    public WarNexus linkedWarNexus = null;

    public float linesOffset = 5.0f;
    public float length;

    public void AddPoint()
    {
        GameObject newObj = new GameObject();
        newObj.transform.position = transform.position;
        newObj.name = "WarNexus";
        WarNexus script = newObj.AddComponent<WarNexus>();
        newObj.transform.parent = transform;
        Link(script);
    }

    public WarWay Link(WarNexus linkTo)
    {
        linkTo.linkedWarNexus = this;
        linkedWarNexus = linkTo;
        if(warWay != null)
            for (int i = 0; i < linkTo.warWay.linkedNexuses.Length; i++)
            {
                if(this == linkTo.warWay.linkedNexuses[i])
                {
                    warWay = linkTo.warWay;
                }
            }
        if (warWay == null)
            warWay = new WarWay();
        warWay.LinkNexuses(this, linkTo);
        return warWay;

        /*
        for (int i = 0; i < warWay.Count; i++)
        {
            var link = warWay[i];
            for (int q = 0; q < link.linkedNexuses[q].length; q++)
                if (link.linkedNexuses[q] == linkTo)
                {
                    ww = link;
                    ww.LinkNexuses(this, linkTo);
                }
        }
        if(ww == null)
        {
            ww = new WarWay();
            warWay.Add(ww);
        }
        for (int i = 0; i < linkTo.warWay.Count; i++)
        {
            var link = linkTo.warWay[i];
            for (int q = 0; q < link.linkedNexuses[q].length; q++)
                if(link.linkedNexuses[q] == this)
                {
                    linkTo.warWay.Remove(link);
                }
        }
        ww.LinkNexuses(this, linkTo);
        if (!this.linkedWarNexus.Contains(linkTo))
            this.linkedWarNexus.Add(linkTo);
        if (!linkTo.linkedWarNexus.Contains(this))
            linkTo.linkedWarNexus.Add(this);
            */
    }
}