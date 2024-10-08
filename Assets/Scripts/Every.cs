using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Every: MonoBehaviour
{
    private bool up = false;
    private int mode;
    private float speedGoUp;
    private int[] listBalloons;
    public float setSettings(float i, int num, Vector3 point, int mode1, float magnitude) // mode = 1 - square; mode = 0 - balloon
    {
        mode = mode1;
        if (mode1 == 1){
            var sc = (1.6f + num * 0.7f) * magnitude;
            transform.localScale = new Vector3(sc, sc, 1);
            listBalloons = new int[num];
            for(int k = 0; k<num; k++)
            {
                listBalloons[k] = -1;
            }
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x*magnitude, transform.localScale.y * magnitude);
        }
        transform.position = new Vector3(point.x + 1.4f * i + transform.localScale.x / 2, point.y + transform.localScale.y / 2, point.z);

        return transform.localScale.x;
    }
    public Vector3 getPos()
    {
        return transform.position;
    }
    public void setPos(Vector2 pos)
    {
        transform.position = pos;
    }
    public float getScale()
    {
        return transform.localScale.x;
    }
    public void Up(float speedGoUp1)
    {
        up = true;
        speedGoUp = speedGoUp1;
    }
    public bool getUp()
    {
        return up;
    }
    public int[] getListBalloons()
    {
        return listBalloons;
    }
    public void setBalloonInList(int index, int value)
    {
        listBalloons[index] = value;
    }
    private void Update()
    {
        if (up)
        {
            transform.position += new Vector3(0, speedGoUp, 0);
        }
    }
}
