using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public Transform windDir;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.eulerAngles = new Vector3(windDir.eulerAngles.x, windDir.eulerAngles.y+180, windDir.eulerAngles.z);
    }
}
