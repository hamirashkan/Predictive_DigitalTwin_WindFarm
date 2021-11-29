
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DragDropObjects : MonoBehaviour
{
    [SerializeField]
    public GameObject[] placeableObjectPrefabs;
    private GameObject[] ObjCollider;
    private GameObject currentPlaceableObject, rayHitObj;

    public TMP_InputField /*InputWS, InputWD,*/ InputBL, InputCp, InputWakeLoss, InputMechLoss, InputElecLoss, InputTimeOut;
    public Slider /*SliderWS, SliderWD,*/ SliderBL, SliderCp, SliderWakeLoss, SliderMechLoss, SliderElecLoss, SliderTimeOut;

    public Transform GaugePanel, TurbineController;
    private int currentPrefabIndex = -1;
    public static float RPM, WindPower;
    public static string Name;
    private float BladeLen, WakeLoss, TimeOut, MechLoss, ElecLoss, Cp, mouseWheelRotation;
    public Toggle toggle;
    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gaugeActive();
        
        if (rayHitObj != null)
        {
            TurbineData();
            WindControllActivate();
        }

        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            ReleaseIfClicked();
        }

        DeletingItem();
    }

    private void TurbineData()
    {
        RPM = rayHitObj.GetComponent<TurbineSetting>().RPM;
        WindPower = rayHitObj.GetComponent<TurbineSetting>().WindOutPut;
        if (flag)
        {
            rayHitObj.GetComponent<TurbineSetting>().BladeLength = SliderBL.value;
            rayHitObj.GetComponent<TurbineSetting>().Cp = SliderCp.value;
            rayHitObj.GetComponent<TurbineSetting>().WakeLoss = SliderWakeLoss.value;
            rayHitObj.GetComponent<TurbineSetting>().MechLoss = SliderMechLoss.value;
            rayHitObj.GetComponent<TurbineSetting>().ElecLoss = SliderElecLoss.value;
            rayHitObj.GetComponent<TurbineSetting>().TimeOut = SliderTimeOut.value;
            test();
        }
        else
        {
            SliderBL.value = rayHitObj.GetComponent<TurbineSetting>().BladeLength;
            SliderCp.value = rayHitObj.GetComponent<TurbineSetting>().Cp;
            SliderWakeLoss.value = rayHitObj.GetComponent<TurbineSetting>().WakeLoss;
            SliderMechLoss.value = rayHitObj.GetComponent<TurbineSetting>().MechLoss;
            SliderElecLoss.value = rayHitObj.GetComponent<TurbineSetting>().ElecLoss;
            SliderTimeOut.value = rayHitObj.GetComponent<TurbineSetting>().TimeOut;

        }

    }
    public void InputFieldUpdate()
    {   
        InputBL.text = SliderBL.value.ToString();
        InputCp.text = SliderCp.value.ToString();
        InputWakeLoss.text = SliderWakeLoss.value.ToString();
        InputMechLoss.text = SliderMechLoss.value.ToString();
        InputElecLoss.text = SliderElecLoss.value.ToString();
        InputTimeOut.text = SliderTimeOut.value.ToString();
    }
    public void SliderUpdate()
    { 
        SliderBL.value = float.Parse(InputBL.text);
        SliderCp.value = float.Parse(InputCp.text);
        SliderWakeLoss.value = float.Parse(InputWakeLoss.text);
        SliderMechLoss.value = float.Parse(InputMechLoss.text);
        SliderElecLoss.value = float.Parse(InputElecLoss.text);
        SliderTimeOut.value = float.Parse(InputTimeOut.text);
    }

    private void test()
    {
        InputBL.text = SliderBL.value.ToString();
        InputCp.text = SliderCp.value.ToString();
        InputWakeLoss.text = SliderWakeLoss.value.ToString();
        InputMechLoss.text = SliderMechLoss.value.ToString();
        InputElecLoss.text = SliderElecLoss.value.ToString();
        InputTimeOut.text = SliderTimeOut.value.ToString();
    }


    private bool PressedKeyOfCurrentPrefab()
    {
        return currentPlaceableObject != null && currentPrefabIndex == 0;
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 5f);
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;

            ObjCollider = GameObject.FindGameObjectsWithTag("Clone");
            foreach (GameObject Clone in ObjCollider)
            {
                Clone.GetComponent<Collider>().enabled = true;
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentPlaceableObject);
        }
    }
    public string DTtoGameObjectName(DateTime dt)
    {
        string sp = "-";
        string mp = "_";
        return dt.Year.ToString() + sp + dt.Month.ToString().PadLeft(2, '0') + sp + dt.Day.ToString().PadLeft(2, '0') + mp + dt.Hour.ToString().PadLeft(2, '0') + sp + dt.Minute.ToString().PadLeft(2, '0') + sp + dt.Second.ToString().PadLeft(2, '0');// + sp + dt.Millisecond.ToString().PadLeft(3, '0');
    }

    public void instantiationWindTurbine()
    {

        if (PressedKeyOfCurrentPrefab())
            {
                Destroy(currentPlaceableObject);
                currentPrefabIndex = -1;
            }
        else
            {
                if (currentPlaceableObject != null)
                {
                    Destroy(currentPlaceableObject);
                }

                currentPlaceableObject = Instantiate(placeableObjectPrefabs[0]);
                //currentPlaceableObject.GetComponent<Collider>().enabled = true;
                currentPlaceableObject.tag = "prefab";
                //currentPlaceableObject.layer = 8;
                currentPlaceableObject.name = "WindTurbine" + "_" + DTtoGameObjectName(DateTime.Now);
                currentPlaceableObject.transform.parent = GameObject.Find("WindFarm").transform;
                //currentPlaceableObject.AddComponent<TurbineSetting>();
            }
        }


    public void instantiationOilRig()
    {

        if (PressedKeyOfCurrentPrefab())
        {
            Destroy(currentPlaceableObject);
            currentPrefabIndex = -1;
        }
        else
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }

            currentPlaceableObject = Instantiate(placeableObjectPrefabs[1]);
            currentPlaceableObject.tag = "Clone";
            currentPlaceableObject.name = "OilRig" + "_" + DTtoGameObjectName(DateTime.Now);
            currentPlaceableObject.transform.parent = GameObject.Find("WindFarm").transform;
        }
    }


    private void gaugeActive()
    {
        var LayerMask = 1 << 8;
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity, LayerMask)) 
            {
                GaugePanel.gameObject.SetActive(false);
                rayHitObj = hitInfo.transform.parent.gameObject;
                Name = rayHitObj.name;
                GaugePanel.gameObject.SetActive(true);
            }
        }
        if (Input.GetMouseButton(1))
        {

            TurbineController.gameObject.SetActive(false);
            GaugePanel.gameObject.SetActive(false);

        }
    }

    private void WindControllActivate()
    {

        if (toggle.isOn)
        {
            TurbineController.gameObject.SetActive(true);
            flag = true;
        }
        else
        {
            TurbineController.gameObject.SetActive(false);
            flag = false;
        }
    }

    private void DeletingItem()
    {

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (Input.GetKeyUp(KeyCode.Delete))
                {
                    //hitInfo.collider.gameObject.GetComponent<Renderer>().enabled = !(hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled);
                    if (hitInfo.collider.gameObject.layer == 4)//GameObject.FindGameObjectWithTag("Ocean"))
                    {
                        Debug.Log("The terrain cannot be deleted!");
                    }
                    else
                    {
                        Destroy(hitInfo.collider.transform.parent.gameObject);
                        GaugePanel.gameObject.SetActive(false);
                    }
                }
            }

        }
    }

    //void OnSceneGUI(SceneView view)
    //{
    //    Event e = Event.current;
    //    GameObject go = HandleUtility.PickGameObject(e.mousePosition, true);
    //    // Do what you must with the go selected
    //    Debug.Log(go);
    //}
}

