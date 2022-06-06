using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindArea : MonoBehaviour
{
    [Range(1,50)]
    public float WindSpeed;
    [Range(0, 360)]
    public float  WindDirect;
    //public Vector3 direction;
    public static float WindSpeedValue;
    public Material OceanMaterial;
    public Slider SliderWS, SliderWD;
    public TMP_InputField InputWS, InputWD;
    public ParticleSystem WindParticle;
    public WindZone zone;
    public TMP_Text servotext, commandtxt;


    void Start()
    {
        WindSpeed = 15;
        WindDirect = 0;
        //var tempRotation = Quaternion.identity;
        //var tempVector = Vector3.zero;
        //tempVector = tempRotation.eulerAngles;
        //tempVector.y = Random.Range(0, 359);
        //tempRotation.eulerAngles = tempVector;
        //transform.rotation = tempRotation;
    }

    [System.Obsolete]
    void Update()
    {
        servotext.text = ("Servo Motor Position: "+OPC_UA.Feedback_OPCUA.ToString());
        commandtxt.text = ("Giving command " + OPC_UA.Command_OPCUA.ToString());
        OceanMaterial.SetFloat("_Wave_Strength", WindSpeed * 0.15f);
        WindSpeedValue = WindSpeed;
        var trail = WindParticle.trails;
        trail.ratio = WindSpeed / 50.0f;
        WindParticle.maxParticles = (int)(WindSpeed * 100);
        zone.windMain = (WindSpeed / 10);



        this.transform.eulerAngles = new Vector3(0, WindDirect,0);
        if (Input.GetMouseButton(0) || Input.anyKey)
        {
            if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>())
            {
                Active_InputField();
            }
            else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Slider>())
            {
                Active_sliders();
            }
        }
    }

    private void Active_InputField()
    {
        WindDirect = float.Parse(InputWD.text);
        SliderWD.value = WindDirect;
        WindSpeed = float.Parse(InputWS.text);
        SliderWS.value = WindSpeed;
    }

    private void Active_sliders()
    {
        WindDirect = SliderWD.value;
        InputWD.text = WindDirect.ToString();
        WindSpeed = SliderWS.value;
        InputWS.text = WindSpeed.ToString();
    }

}

