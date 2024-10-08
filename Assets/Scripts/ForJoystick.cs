using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForJoystick : MonoBehaviour
{
    private bool onClick = false;
    public void OnMouseEnter()
    {
        onClick = true;
        Debug.Log("xxx");
    }


    public bool getOnClick()
    {
        return onClick;
    }
    public Vector3 getPos()
    {
        return transform.position;
    }
    public void setPos(Vector2 pos)
    {
        transform.position = pos;
    }
}
