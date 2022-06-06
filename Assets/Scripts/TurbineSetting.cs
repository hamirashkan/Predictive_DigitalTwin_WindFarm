using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurbineSetting : MonoBehaviour
{

    //public Button FMU_Button;
    private GameObject WindTransform;
    private Transform YawObj, HubObj, BladeObj, barPanel, warningSign;
    [Range(1.0f, 100.0f)]
    public float BladeLength, HubLength;
    [Range(1.0f, 100.0f)]
    public float WakeLoss, TimeOut, MechLoss, ElecLoss, Cp;
    private float windout, rpm, Powerrate, RPMrate, angle, meter, TempRate;
    private int clickNumber, TempTest = 0;
    public float WindOutPut, RPM, windDirection, csvTemp,csvMax, csvMin;
    private TextMeshPro BarText,TempBarText,minText,maxText;
    public static string Title_GUI;
    public static float WindOutputValue, RPMValue, WindDirectionValue, WindSpeedValue, BladeLengthValue;
    //public KeyCode _key;
    private Button yourbutton;
    private Toggle toggleTempMode, togglePowerMode;
    private Color defaulColor;
    private bool tempFlag = false;
    public DragDropObjects forcastFunc;
    public string plotValue;
    [SerializeField] private float _duration = 10f;
    private float _timer = 0f;


    private void Start()
    {
        Powerrate = 0f; RPMrate = 0f;
        var bar = this.gameObject.transform.GetChild(0).GetChild(1);
        
        var minVal = this.gameObject.transform.GetChild(4).GetChild(0).GetChild(0);
        var maxVal = this.gameObject.transform.GetChild(4).GetChild(0).GetChild(1);
        var TempBar = this.gameObject.transform.GetChild(4).GetChild(0).GetChild(2).GetChild(1);
        barPanel = this.gameObject.transform.GetChild(4).GetChild(0);
        warningSign = this.gameObject.transform.GetChild(4).GetChild(0).GetChild(3);
        minText = minVal.GetComponent<TextMeshPro>();
        maxText = maxVal.GetComponent<TextMeshPro>();
        BarText = bar.GetComponent<TextMeshPro>();
        TempBarText = TempBar.GetComponent<TextMeshPro>();
        YawObj = this.transform.Find("Yaw");
        HubObj = this.transform.Find("Tower");
        BladeObj = this.YawObj.Find("Blades");
        WindTransform = GameObject.FindGameObjectWithTag("WindArea");
        BladeLength = 52; HubLength = 1.0f; Cp = 4;
        yourbutton = GameObject.Find("DataSwap").GetComponent<Button>();
        Button btn = yourbutton.GetComponent<Button>();
        btn.onClick.AddListener(clicknumber);
        
        defaulColor = this.gameObject.transform.GetChild(3).GetChild(1).GetComponentInChildren<Renderer>().material.color;
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
        csvAllocation(this.gameObject.name);
        warningAlarm(csvTemp);
        //WindPowerCalculation(BladeLength, WindArea.WindSpeedValue);
        this.transform.eulerAngles = new Vector3(0, windDirection, 0);
        YawObj.localScale = new Vector3((1+ BladeLengthValue * 0.02f), (1+ BladeLengthValue * 0.02f), (1+ BladeLengthValue * 0.01f));
        HubObj.localScale = new Vector3(1, HubLength, 1);
        BladeObj.Rotate(new Vector3(0, 0, (float)(RPM * 5.0f)) * Time.deltaTime);
        //Pitch.transform.eulerAngles = new Vector3(0, PitchAngle, 0);
        //transform.Rotate(Vector3.forward, (RPM * 6f) * Time.deltaTime);
    }

    void clicknumber()
    {
        clickNumber += 1;
        if (clickNumber > 3)
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
        TempAnalogFormat(csvTemp);
    }
    void FMU_Data() 
    {
        WindSpeedValue = 12;
        RPM = RpmAnalogFormat(WindFarmFMU.RPM_FMU);
        WindOutPut = PowerAnalogFormat(WindFarmFMU.POWER_FMU);
        windDirection = WindDirectAnalogFormat(WindTransform.transform.eulerAngles.y);
        Title_GUI = "FMU";
        yourbutton.GetComponent<Image>().color = new Color32(247, 219, 106, 255);
        BladeLengthValue = BladeAnalogFormat(WindFarmFMU.Blade_FMU);
        TempAnalogFormat(csvTemp);
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
        TempAnalogFormat(csvTemp);
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

    float TempAnalogFormat(float Temp)
    {
        if (TempRate < RPM)
        {
            TempRate += 15f * Time.deltaTime;
        }
        if (TempRate > RPM)
        {
            TempRate -= 15f * Time.deltaTime;
        }
        return TempRate;
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

            case 3:
                Unity_Data();
                yourbutton.GetComponentInChildren<Text>().text = "Actual Data";
                break;

            default:
                break;
        }
        
        //set clickNumber to 0 when greater than the number of methods

    }

    public void csvAllocation(string name)
    {
        if( name == "WindTurbine (1)") { csvTemp = CSVReader.v1 + TempTest; plotValue = "plotValue1.bat"; csvMax = CSVReader.maxValue0; csvMin = CSVReader.minValue0;}
        else if (name == "WindTurbine (2)") { csvTemp = CSVReader.v2; plotValue = "plotValue2.bat"; csvMax = CSVReader.maxValue1; csvMin = CSVReader.minValue1; }
        else if(name == "WindTurbine (3)") { csvTemp = CSVReader.v3; plotValue = "plotValue3.bat"; csvMax = CSVReader.maxValue2; csvMin = CSVReader.minValue2; }
        else if(name == "WindTurbine (4)") { csvTemp = CSVReader.v4; plotValue = "plotValue4.bat"; csvMax = CSVReader.maxValue3; csvMin = CSVReader.minValue3; }
        else if(name == "WindTurbine (5)") { csvTemp = CSVReader.t5; plotValue = "plotValue5.bat"; csvMax = CSVReader.maxValue4; csvMin = CSVReader.minValue4; }
        else if(name == "WindTurbine (6)") { csvTemp = CSVReader.t6; plotValue = "plotValue6.bat"; csvMax = CSVReader.maxValue5; csvMin = CSVReader.minValue5; }
        else if(name == "WindTurbine (7)") { csvTemp = CSVReader.t7; plotValue = "plotValue7.bat"; csvMax = CSVReader.maxValue6; csvMin = CSVReader.minValue6; }
        else if(name == "WindTurbine (8)") { csvTemp = CSVReader.t8; plotValue = "plotValue8.bat"; csvMax = CSVReader.maxValue7; csvMin = CSVReader.minValue7; }
        else if (name == "WindTurbine (9)") { csvTemp = CSVReader.t9; plotValue = "plotValue9.bat"; csvMax = CSVReader.maxValue8; csvMin = CSVReader.minValue8; }
        else if (name == "WindTurbine (10)"){ csvTemp = CSVReader.t10;plotValue = "plotValue10.bat";csvMax = CSVReader.maxValue9; csvMin = CSVReader.minValue9; }
        else if (name == "WindTurbine (11)"){ csvTemp = OPC_UA.WindTemp_OPCUA; plotValue = "plotValue11.bat"; csvMax = CSVReader.maxValue10; csvMin = CSVReader.minValue10; }

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
        var dist = Vector3.Distance(this_pos, windzone_dist)/6;
        //Debug.Log("Name: " + this.transform.name + "Distance" + diff);
        WakeLoss = dist;
        var rho = 1.23f;  //Air density (kg / m3)
        // Power Coefficient the real world limit is well below the Betz Limit with values of 0.35 - 0.45 common even in the best designed wind turbines
        var Radius = BladeLength;   //Blade Length(m)
        var Miu =(1 - (WakeLoss * 0.01)) * (1 - (MechLoss * 0.01)) * (1 - (TimeOut * 0.01)) * (1 - (ElecLoss * 0.01)) * (Cp * 0.01); //Real efficiency (1 - ((diff) * 0.01)) * 
        var SweptArea = (Math.PI * Math.Pow(Radius, 2));  //Swept Are (m2)
        var WindPower = (0.5f * rho * SweptArea * Math.Pow(WindVelocity, 3));   // PowerWind = 1/2 * rho * A * V**3 * Cp //(Wind Speed m/s) 
        windout = (float)(Miu * WindPower);
        var RotorDiameter = BladeLength * 2; //Rotor Diameter
        var TSR = 8;  //Tip Speed Ratio
        //rpm = (float)(1 - ((dist) * 0.01)) * (float)((60 * WindVelocity * TSR) / (Math.PI * RotorDiameter));
        rpm = (float)((60 * WindVelocity * TSR) / (Math.PI * RotorDiameter));
        //PowerText.text = ("Wind Power: " + (WindOutPut / 1000000).ToString("0.00") + " MW");
        //RPMText.text = ("Rotational Speed: " + RPM.ToString("0.00") + " rpm");
    }

    //[Obsolete]
    void indicator()
    {
        

        //var ind = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
        //var nacelle1 = this.gameObject.transform.GetChild(3).GetChild(2).GetChild(0);
        //var nacelle2 = this.gameObject.transform.GetChild(3).GetChild(2).GetChild(1);
        //var nacelle3= this.gameObject.transform.GetChild(3).GetChild(2).GetChild(2);

        //var hingeJoints = this.gameObject.transform.GetChild(3).GetChild(1).GetComponentsInChildren<Renderer>();
        var power = WindOutPut / 100000;
        if (tempFlag)
        {
            SetGauge((16f * power / 100));
        }

        if (GameObject.Find("ToggleTempMode") != null)
        {
            toggleTempMode = GameObject.Find("ToggleTempMode").GetComponent<Toggle>();
            togglePowerMode = GameObject.Find("TogglePowerMode").GetComponent<Toggle>();
            
            if (toggleTempMode.isOn)
            {
                this.transform.GetChild(4).gameObject.SetActive(true);
                //foreach (Renderer r in hingeJoints)
                //{
                //    r.material.color = new Color(csvTemp / 220, 1 - csvTemp / 120, 0, 1f);
                //}
            }
            else
            {
                this.transform.GetChild(4).gameObject.SetActive(false);
                //foreach (Renderer r in hingeJoints)
                //{

                //    r.material.color = defaulColor;
                //}
            };
            if (togglePowerMode.isOn)
            {
                tempFlag = true;
                this.transform.GetChild(5).gameObject.SetActive(true);
            }
            else
            {
                tempFlag = false;
                this.transform.GetChild(5).gameObject.SetActive(false);

            };
        }


        var name = this.gameObject.name;
        //child.transform.localScale = new Vector3(10, 20, 10);
        //origin.transform.localScale = new Vector3(1.6666f, 0.07f * power / 0.5f, 1.6666f);
        //ind.transform.GetComponent<Renderer>().material.color = new Color(1 - 15f * power / 100, 15f * power / 100, 0,1f);
        var origin = barPanel.transform.GetChild(2).GetChild(0);
        var tempIndic = barPanel.transform.GetChild(2).GetChild(0).GetChild(0);

        tempIndic.transform.GetComponent<Renderer>().material.color = new Color((csvTemp-60) / 60, 1 - ((csvTemp-60) / 60), 0, 1f);
        origin.transform.localScale = new Vector3(1.6666f, ((csvTemp - 60) / 60), 1.6666f);
        TempBarText.text = csvTemp.ToString("0.00" +
            "") + " °C";
        TempBarText.transform.rotation = Quaternion.LookRotation(BarText.transform.position - Camera.main.transform.position);
        TempBarText.color = new Color((csvTemp - 60) / 60, 1 - ((csvTemp - 60) / 60), 0, 1f);

        minText.text = "Min: " + csvMin.ToString("0.0");
        maxText.text = "Max: " + csvMax.ToString("0.0");
        barPanel.transform.rotation  = Quaternion.LookRotation(BarText.transform.position - Camera.main.transform.position);
        //maxText.transform.rotation = Quaternion.LookRotation(BarText.transform.position - Camera.main.transform.position);
        //BarText.text = (CSVReader.t1.ToString("0.0") + " °C");
        //BarText.color = new Color(CSVReader.t1 / 120, 1- CSVReader.t1 / 120, 1f , 1f);
        //BarText.transform.localScale = new Vector3(0.6f + (0.0033f * CSVReader.t1), 0.6f + (0.0033f * CSVReader.t1), 1f);
        //BarText.transform.rotation = Quaternion.LookRotation(BarText.transform.position - Camera.main.transform.position);
        //Nacelle.color = new Color(16f * power / 100, 1 - 16f * power / 100, 1f, 1f);
    }

    public void SetGauge(float percent)
    {
        //GameObject gaugeGO = gameObject.transform.parent.parent.gameObject;
        Transform gaugeGO = this.gameObject.transform.GetChild(5);
        gaugeGO.SendMessage("SetPercentage", percent);
        gaugeGO.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - gaugeGO.transform.position);
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
    private void warningAlarm(float csvtemp)
    {
        


        var hingeJoints = this.gameObject.transform.GetChild(3).GetChild(1).GetComponentsInChildren<Renderer>();
        if (csvtemp > csvMax)
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration || csvtemp > csvMax + 10)
            {
                RPM = 0;
                WindOutPut = 0;
                Debug.Log("WARNING!!!");
            }
                warningSign.transform.gameObject.SetActive(true);
            //System.Diagnostics.Process.Start("C:\\Users\\Amirashkan\\Desktop\\plot.bat");
            //Debug.Log("The temperature is increasing " + csvtemp +" the max value is " + csvMax);
            foreach (Renderer r in hingeJoints)
            {
                r.material.color = Color.red;
            }
        }
        else if(csvtemp < csvMin)
        {
            _timer = 0;
            foreach (Renderer r in hingeJoints)
            {
                r.material.color = Color.blue;
            }
        }
        else 
        {
            _timer = 0;
            warningSign.transform.gameObject.SetActive(false);
            foreach (Renderer r in hingeJoints)
            {
                r.material.color = Color.white;
            }

        }

    }


    public void PlusTemp()
    {
        TempTest += 1;
        Debug.Log("tempTest: "+ TempTest);
    }
    public void MinusTemp()
    {
        TempTest -= 1;
        Debug.Log("tempTest: " + TempTest);
    }
}
