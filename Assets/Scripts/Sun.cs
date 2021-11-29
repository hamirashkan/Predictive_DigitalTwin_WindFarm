using System;
using UnityEngine;
using UnityEngine.UI;

public class Sun : MonoBehaviour
{


    public GameObject Target;
    public Slider SliderDis;
    public Text Daily_text;
    public Text Sun_Speed;

    public static float hour;
    private float minute;
    public float speed;
    public static float speed2;
    public static int Hour;
    public static bool Sun_Pos = true;
    public static float Delta_time;
    public static float Slider;
    private float New_time;
    public static Vector3 Default_pos;



    public Slider SlideBar_Speed;

    private string currentCaseTimeText;



    //void setText_sun()
    //{
    //    Daily_text.text = ("Day time: " + currentCaseTimeText);
    //    Sun_Speed.text = ("Sun Speed: " + SlideBar_Speed.value.ToString("0"));
    //}


    void Start()
    {
        hour = 0;
        minute = 0;
        //Default_pos = Target.transform.position = new Vector3((-1500), 500, (1500));
    }



    //public void Day_time()
    //{
    //    hour = (Mathf.Abs(Convert.ToSingle(SliderDis.value) / 250f))+6;
    //    currentCaseTimeText = String.Format("{0:00}:{1:00}", hour, minute);
    //    Hour = Convert.ToInt32(hour);

    //   //Debug.Log("Hour = " + hour);
    //}


    void Update()
    {
        //Slider = SliderDis.value;
        speed2 = speed;
        //setText_sun();
        //Day_time();
        Sun_Rotation();
        //AdjustSpeed();
        Sun_position();

        //if (Target.transform.position.y <= 0)
        //{
        //    Sun_Pos = false;
        //    Target.transform.position = Default_pos;
        //}
        //else
        //{
        //    Sun_Pos = true;
        //}


    }
    //public void AdjustSpeed()
    //{
    //    speed = (int)SlideBar_Speed.value*2;
    //}

    public void Sun_position()
    {
        Target.transform.position = new Vector3(0 , 500, 0);
        transform.LookAt(Vector3.zero);

        //Delta_time = SliderDis.value - New_time;
       // New_time = SliderDis.value;
        //Debug.Log("delta time" + Delta_time);


    }

    public void Sun_Rotation()
    {

        hour = (Mathf.Abs(Convert.ToSingle(this.transform.position.x + 1500) / 250f)) + 6;
        currentCaseTimeText = String.Format("{0:00}:{1:00}", hour, minute);
        Hour = Convert.ToInt32(hour);
        //Debug.Log("Hour = " + hour);

        transform.RotateAround(Vector3.zero, new Vector3(0,0,1), -speed * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }

}
