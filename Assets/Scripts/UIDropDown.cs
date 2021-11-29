using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDropDown : MonoBehaviour
{

    //public TextMeshProUGUI output;
    public GameObject Inventory, WindController, TurbineController;

    [System.Obsolete]

    public void HandleInputData(int val)
    {
        if(val == 0)
        {
            WindController.transform.gameObject.active = false;
            TurbineController.transform.gameObject.active = false;
            Inventory.transform.gameObject.active = false;
        }
        
        if (val == 1)
        {
            WindController.transform.gameObject.active = false;
            TurbineController.transform.gameObject.active = false;
            Inventory.transform.gameObject.active = true;
        }
        if (val == 2)
        {
            Inventory.transform.gameObject.active = false;
            TurbineController.transform.gameObject.active = false;
            WindController.transform.gameObject.active = true;
        }
        if (val == 3)
        {
            Inventory.transform.gameObject.active = false;
            WindController.transform.gameObject.active = false;
            TurbineController.transform.gameObject.active = true;
        }
    }
    void Start()
    {
 
    }


}
