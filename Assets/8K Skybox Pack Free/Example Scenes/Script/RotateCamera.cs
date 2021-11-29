using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField]
    private float lookSpeedH = 5f;

    [SerializeField]
    private float lookSpeedV = 5f;

    [SerializeField]
    private float zoomSpeed = 10f;

    [SerializeField]
    private float dragSpeed = 5f;

    private float yaw = 0f;
    private float pitch = 0f;

    private Vector3 default_pos;
    private Vector3 default_rot;


    // Use this for initialization
    void Start()
    {
        this.yaw = this.transform.eulerAngles.y;
        this.pitch = this.transform.eulerAngles.x;
        default_pos = this.transform.position;
        default_rot = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(this.dragSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-this.dragSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -this.dragSpeed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, this.dragSpeed * Time.deltaTime));

        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            //Look around with Left Mouse
            if (Input.GetMouseButton(0))
            {
                this.yaw += this.lookSpeedH * Input.GetAxis("Mouse X");
                this.pitch -= this.lookSpeedV * Input.GetAxis("Mouse Y");

                this.transform.eulerAngles = new Vector3(this.pitch, this.yaw, 0f);
            }

            //drag camera around with Middle Mouse
            if (Input.GetMouseButton(2))
            {
                transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
            }

            this.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * this.zoomSpeed, Space.Self);
        }
        
    }
}

