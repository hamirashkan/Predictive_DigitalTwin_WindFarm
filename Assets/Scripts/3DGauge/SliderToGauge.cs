using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderToGauge : MonoBehaviour {

    public void SetGauge(float percent)
    {
        //GameObject gaugeGO = gameObject.transform.parent.parent.gameObject;
        GameObject gaugeGO = GameObject.FindGameObjectWithTag("3DGauge");
        gaugeGO.SendMessage("SetPercentage", percent);
    }
}
