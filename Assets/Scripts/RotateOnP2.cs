using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnP2 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Game.isPlayer1Turn)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);
        } else
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 180.0f);
        }
    }
}
