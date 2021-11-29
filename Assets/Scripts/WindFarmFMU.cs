using FMI2;
using UnityEngine;
using UnityEngine.UI;

public class WindFarmFMU : MonoBehaviour
{
    public float stepTime = 1.0f;
    private FMU fmu;

    [Range(1, 100)]
    public float initWindSpeed = 12;
    [Range(1, 100)]
    public float initBladeLength = 52;
    private float speed;

    static public float POWER_FMU, RPM_FMU, Blade_FMU;
    public Slider blade_slider;

    //[Range(1, 100)]
    //public float Amplitude = 3;

    //[Range(1, 100)]
    //public float SineWave_Bias = 2;

    //[Range(1, 100)]
    //public float SineWave_Freq = 3;

    //[Range(1, 500)]
    //public float SineWave_Phase = 4;


    void Start()
    {
        speed = 0f;
        // instantiate the FMU
        fmu = new FMU("WindFarm", name);
        Reset();
    }

    private void Update()
    {
        //transform.position += new Vector3(1 * Time.deltaTime, 0, 0);
    }

    public void Reset()
    {
        // reset the FMU
        fmu.Reset();
        // start the experiment at the current time
        fmu.SetupExperiment(Time.time);
        fmu.EnterInitializationMode();
        //fmu.SetReal("BladeLength", blade_slider.value);
        fmu.ExitInitializationMode();
    }

    void FixedUpdate()
    {
        fmu.SetReal("BladeLength", blade_slider.value);
        // synchronize the model with the current time
        fmu.DoStep(Time.time, stepTime);// Time.deltaTime);

        // get the variable "h" (height)
        //transform.position = Vector3.up * (float)fmu.GetReal("PowerOutput");
        //if (speed < (float)fmu.GetReal("RPM"))
        //{
        //    speed += 50f * Time.deltaTime;
        //}
        //if (speed > (float)fmu.GetReal("RPM"))
        //{
        //    speed -= 50f * Time.deltaTime;
        //}
        //RPM_FMU = speed;
        RPM_FMU = (float)fmu.GetReal("RPM");
        POWER_FMU = (float)fmu.GetReal("PowerOutput")*10;
        Blade_FMU = (float)fmu.GetReal("BladeLength");
        this.transform.Rotate(new Vector3(0, 0, speed * 6 * Time.deltaTime));
        //this.transform.Rotate(new Vector3(0, ((float)fmu.GetReal("WindInput") * 6.0f), 0) * Time.deltaTime);

    }

    void OnDestroy()
    {
        // clean up
        fmu.Dispose();
    }


    //void OnGUI()
    //{

    //    GUIStyle mystyle2 = new GUIStyle(GUI.skin.button);
    //    mystyle2.fontSize = 25;
    //    mystyle2.alignment = TextAnchor.MiddleLeft;
    //    mystyle2.fontStyle = FontStyle.Bold;
    //    mystyle2.normal.textColor = Color.red;

    //    GUI.backgroundColor = Color.black;

    //    GUIStyle mystyle = new GUIStyle(GUI.skin.button);
    //    mystyle.fontSize = 25;
    //    mystyle.alignment = TextAnchor.MiddleLeft;
    //    mystyle.fontStyle = FontStyle.Bold;
    //    mystyle.normal.textColor = Color.yellow;


    //    GUI.Box(new Rect(10, 80, 200, 40), (blade_slider.value).ToString() + " m", mystyle);
    //    GUI.Box(new Rect(10, 120, 200, 40), fmu.GetReal("RPM").ToString("0.00" + " RPM"), mystyle);
    //    GUI.Box(new Rect(10, 160, 200, 40), (fmu.GetReal("PowerOutput") / 10000).ToString("0.00" + " MW"), mystyle);

    //}
}
