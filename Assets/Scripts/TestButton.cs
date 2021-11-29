using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TestButton : MonoBehaviour
{
    public static int num;
    public Text Indicator;
    void Start()
    {
        num = 26;
        Indicator.text = num.ToString();
     
    }
    
    public void Increase()
    {
        num += 1;
        Indicator.text = num.ToString();
       
        

    }
}
