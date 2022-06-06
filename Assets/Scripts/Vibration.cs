using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 initialPosition;

    private float amplitude; // the amount it moves
    public float frequency; // the period of the earthquake

    void Start()
    {
       
    }



    void FixedUpdate()
    {
        amplitude = CSVReader.v1;
        this.transform.position = transform.up * Mathf.PingPong(frequency , amplitude*100);
    }
}
