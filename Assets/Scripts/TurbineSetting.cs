using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurbineSetting : MonoBehaviour
{

    //public Button FMU_Button;
    private GameObject WindTransform;
    private Transform YawObj, HubObj, BladeObj;
    [Range(1.0f, 100.0f)]
    public float BladeLength, HubLength;
    [Range(1.0f, 100.0f)]
    public float WakeLoss, TimeOut, MechLoss, ElecLoss, Cp;
    private float windout, rpm, Powerrate,RPMrate, angle, meter;
    private int clickNumber;
    public float WindOutPut, RPM, windDirection;
    private TextMeshPro BarText;
    public static string Title_GUI;
    public static float WindOutputValue, RPMValue, WindDirectionValue, WindSpeedValue, BladeLengthValue;
    //public KeyCode _key;
    private Button yourbutton;




    private void Start()
    {
        Powerrate = 0f; RPMrate = 0f;
        var bar = this.gameObject.transform.GetChild(0).GetChild(1);
        BarText = bar.GetComponent<TextMeshPro>();
        YawObj = this.transform.Find("Yaw");
        HubObj = this.transform.Find("Tower");
        BladeObj = this.YawObj.Find("Blades");
        WindTransform = GameObject.FindGameObjectWithTag("WindArea");
        BladeLength = 25; HubLength = 1.0f; Cp = 4;
        yourbutton = GameObject.Find("DataSwap").GetComponent<Button>();
        Button btn = yourbutton.GetComponent<Button>();
        btn.onClick.AddListener(clicknumber);
        //flage = true;
    }


    void Update()
    {
        BladeLengthValue = BladeLength;
        WindOutputValue = WindOutPut;
        RPMValue = RPM;
        WindDirectionValue = windDirection;
        toggle();
        indicator();
        //WindPowerCalculation(BladeLength, WindArea.WindSpeedValue);
        this.transform.eulerAngles = new Vector3(0, windDirection, 0);
        YawObj.localScale = new Vector3((1+ BladeLengthValue * 0.02f), (1+ BladeLengthValue * 0.02f), (1+ BladeLengthValue * 0.01f));
        HubObj.localScale = new Vector3(1, HubLength, 1);
        BladeObj.Rotate(new Vector3(0, 0, (float)(RPM * 6.0f)) * Time.deltaTime);
        //Pitch.transform.eulerAngles = new Vector3(0, PitchAngle, 0);
        //transform.Rotate(Vector3.forward, (RPM * 6f) * Time.deltaTime);
    }

    void clicknumber()
    {
        clickNumber += 1;
        if (clickNumber > 2)
            clickNumber = 0;

    }

    void Unity_Data() 
    {
        WindSpeedValue = WindArea.WindSpeedValue;
        WindPowerCalculation(BladeLength, WindArea.WindSpeedValue);
        RPM = RpmAnalogFormat(rpm);
        WindOutPut = PowerAnalogFormat(windout);
        windDirection = WindDirectAnalogFormat(WindTransform.transform.eulerAngles.y);
        BladeLengthValue = BladeAnalogFormat(BladeLength);
        Title_GUI = "Unity";
        yourbutton.GetComponent<Image>().color = new Color32(247, 139, 106, 255);
    }
    void FMU_Data() 
    {
        WindSpeedValue = 20;
        RPM = RpmAnalogFormat(WindFarmFMU.RPM_FMU);
        WindOutPut = PowerAnalogFormat(WindFarmFMU.POWER_FMU);
        windDirection = WindDirectAnalogFormat(WindTransform.transform.eulerAngles.y);
        Title_GUI = "FMU";
        yourbutton.GetComponent<Image>().color = new Color32(247, 219, 106, 255);
        BladeLengthValue = BladeAnalogFormat(WindFarmFMU.Blade_FMU);
    }
    void OPCUA_Data()
    {
        WindSpeedValue = OPC_UA.WindSpeed_OPCUA;
        WindPowerCalculation(OPC_UA.Blade_OPCUA, OPC_UA.WindSpeed_OPCUA);
        RPM = RpmAnalogFormat(rpm);
        WindOutPut = PowerAnalogFormat(windout);
        //windDirection = OPC_UA.WindDire_OPCUA;
        windDirection = WindDirectAnalogFormat(OPC_UA.WindDir_OPCUA);
        Title_GUI = "OPC_UA";
        yourbutton.GetComponent<Image>().color = new Color32(90, 219, 198, 255);
        BladeLengthValue = BladeAnalogFormat(OPC_UA.Blade_OPCUA);
    }

    float PowerAnalogFormat(float power)
    {
        if (Powerrate < power)
        {
            Powerrate += 100000f * Time.deltaTime;
        }
        if (Powerrate > power)
        {
            Powerrate -= 100000f * Time.deltaTime;
        }
        return Powerrate;
    }

    float RpmAnalogFormat(float RPM)
    {
        if (RPMrate < RPM)
        {
            RPMrate += 15f * Time.deltaTime;
        }
        if (RPMrate > RPM)
        {
            RPMrate -= 15f * Time.deltaTime;
        }
        return RPMrate;
    }


    float WindDirectAnalogFormat(float WindDirect)
    {
        if (angle < WindDirect)
        {
            angle += 30f * Time.deltaTime;
        }
        if (angle > WindDirect)
        {
            angle -= 30f * Time.deltaTime;
        }
        return angle;
    }
    float BladeAnalogFormat(float BladeLength)
    {
        if (meter < BladeLength)
        {
            meter += 10f * Time.deltaTime;
        }
        if (meter > BladeLength)
        {
            meter -= 10f * Time.deltaTime;
        }
        return meter;
    }


    public void toggle()
    {
        //if(flage ^= true)
        //if (Input.GetKeyDown(KeyCode.Tab))
        //if (Input.GetButtonDown("DataSwap"))
        switch (clickNumber)
        {
            case 0:
                Unity_Data();
                yourbutton.GetComponentInChildren<Text>().text = "Unity Data";
                break;

            case 1:
                FMU_Data();
                yourbutton.GetComponentInChildren<Text>().text = "FMU Data";
                break;

            case 2:
                OPCUA_Data();
                yourbutton.GetComponentInChildren<Text>().text = "OPCUA Data";
                break;

            default:
                break;
        }
        
        //set clickNumber to 0 when greater than the number of methods

    }

    //void OnGUI()
    //{
    //    GUIStyle mystyle2 = new GUIStyle(GUI.skin.button);
    //    mystyle2.fontSize = 25;
    //    mystyle2.alignment = TextAnchor.MiddleLeft;
    //    mystyle2.fontStyle = FontStyle.Bold;
    //    mystyle2.normal.textColor = Color.red;

    //    GUI.backgroundColor = Color.grey;

    //    GUIStyle mystyle = new GUIStyle(GUI.skin.button);
    //    mystyle.fontSize = 25;
    //    mystyle.alignment = TextAnchor.MiddleLeft;
    //    mystyle.fontStyle = FontStyle.Bold;
    //    mystyle.normal.textColor = Color.yellow;

    //    GUI.Label(new Rect(10, 360, 200, 40), Title_GUI, mystyle);
    //    GUI.Box(new Rect(10, 410, 200, 40), RPM_GUI, mystyle);
    //    GUI.Box(new Rect(10, 460, 200, 40), POWER_GUI, mystyle);
    //}
    private void  WindPowerCalculation(float BladeLength, float WindVelocity)
    {
        var windzone_dist = GameObject.Find("Particles").transform.position;
        var this_pos = this.transform.position;
        var diff = ((this_pos - windzone_dist).magnitude) / 2;
        //Debug.Log("Name: " + this.transform.name + "Distance" + diff);

        var rho = 1.23f;  //Air density (kg / m3)
        // Power Coefficient the real world limit is well below the Betz Limit with values of 0.35 - 0.45 common even in the best designed wind turbines
        var Radius = BladeLength;   //Blade Length(m)
        var Miu = (1 - ((diff) * 0.01)) * (1 - (WakeLoss * 0.01)) * (1 - (MechLoss * 0.01)) * (1 - (TimeOut * 0.01)) * (1 - (ElecLoss * 0.01)) * (Cp * 0.01); //Real efficiency
        var SweptArea = (Math.PI * Math.Pow(Radius, 2));  //Swept Are (m2)
        var WindPower = (0.5f * rho * SweptArea * Math.Pow(WindVelocity, 3));   // PowerWind = 1/2 * rho * A * V**3 * Cp //(Wind Speed m/s) 
        windout = (float)(Miu * WindPower);
        var RotorDiameter = BladeLength * 2; //Rotor Diameter
        var TSR = 8;  //Tip Speed Ratio
        rpm = (float)(1 - ((diff) * 0.005)) * (float)((60 * WindVelocity * TSR) / (Math.PI * RotorDiameter));
        //PowerText.text = ("Wind Power: " + (WindOutPut / 1000000).ToString("0.00") + " MW");
        //RPMText.text = ("Rotational Speed: " + RPM.ToString("0.00") + " rpm");
    }

    //[Obsolete]
    void indicator()
    {
        
        var origin = this.gameObject.transform.GetChild(0).GetChild(0);
        var ind = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
        var power = WindOutPut / 100000;
        //child.transform.localScale = new Vector3(10, 20, 10);
        origin.transform.localScale = new Vector3(1.6666f, 0.07f * power / 0.5f, 1.6666f);
        ind.transform.GetComponent<Renderer>().material.color = new Color(1 - 15f * power / 100, 15f * power / 100, 0,1f);

        BarText.text = ((WindOutPut / 100000).ToString("0.00") + " MW");
        BarText.color = new Color(16f * power / 100, 1- 16f * power / 100, 1f , 1f);
        BarText.transform.localScale = new Vector3(0.6f + (0.066f * power), 0.6f + (0.066f * power), 1f);
        BarText.transform.rotation = Quaternion.LookRotation(BarText.transform.position - Camera.main.transform.position);
    }
    //private void fixedupdate()
    //{
    //    if (inWindZone)
    //    {

    //        rb.AddForce(windZone.GetComponent<WindArea>().direction * windZone.GetComponent<WindArea>().strength);
    //    }
    //}

    //void ontriggerenter(Collider coll)
    //{
    //    if (coll.gameObject.tag == "windArea")
    //    {
    //        windZone = coll.gameObject;
    //        inWindZone = true;
    //    }
    //}

    //void ontriggerexit(Collider coll)
    //{
    //    if (coll.gameObject.tag == "windArea")
    //    {
    //        inWindZone = false;
    //    }
    //}
}
