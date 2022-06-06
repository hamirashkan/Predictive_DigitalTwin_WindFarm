
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class TempMeter : MonoBehaviour
{
    
    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 230;

    private Transform needleTranform, speedLabelTemplateTransform, gauge;

    private float speedMax;
    private float speed;

    public TextMeshProUGUI TempText;

    private void Awake()
    {

        needleTranform = transform.Find("TempNeedle");
        speedLabelTemplateTransform = transform.Find("TempLabel");
        speedLabelTemplateTransform.gameObject.SetActive(false);

        speed = 0f;
        speedMax = 150f;

        CreateSpeedLabels();
    }

    private void Update()
    {
        HandlePlayerInput();
        //speed += 30f * Time.deltaTime;
        //if (speed > speedMax) speed = speedMax;

        needleTranform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    private void HandlePlayerInput()
    {
        //if(speed < DragDropObjects.WindPower / 100000) 
        //{
        //    speed += 1f * Time.deltaTime;
        //}
        //if(speed > DragDropObjects.WindPower / 100000)
        //{
        //    speed -= 1f * Time.deltaTime;
        //}
        speed = DragDropObjects.Temperature;
        TempText.text = ((DragDropObjects.Temperature).ToString("0.0") + " °C");
        this.gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color((DragDropObjects.Temperature - 60) / 60, 1 - (DragDropObjects.Temperature - 60) / 60, 0, 1f);
        //this.gameObject.transform.GetChild(0).GetComponent<Image>().material.color = new Color(1, 1 , 1, 1f);

        speed = Mathf.Clamp(speed, 0f, speedMax);

    }


    private void CreateSpeedLabels()
    {
        int labelAmount = 10;
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        for (int i = 0; i <= labelAmount; i++)
        {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("TempText").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(labelSpeedNormalized * speedMax).ToString();
            speedLabelTransform.Find("TempText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }

        needleTranform.SetAsLastSibling();
    }

    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float speedNormalized = speed / speedMax;

        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}
