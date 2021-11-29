
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PowerMeter : MonoBehaviour {

    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 230;

    private Transform needleTranform, speedLabelTemplateTransform, gauge;

    private float speedMax;
    private float speed;

    public TextMeshProUGUI PowerText;

    private void Awake() {

        needleTranform = transform.Find("PowerNeedle");
        speedLabelTemplateTransform = transform.Find("PowerLabel");
        speedLabelTemplateTransform.gameObject.SetActive(false);

        speed = 0f;
        speedMax = 20f;

        CreateSpeedLabels();
    }

    private void Update() {
        HandlePlayerInput();
        //speed += 30f * Time.deltaTime;
        //if (speed > speedMax) speed = speedMax;

        needleTranform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }
    
    private void HandlePlayerInput() {
        //if(speed < DragDropObjects.WindPower / 100000) 
        //{
        //    speed += 1f * Time.deltaTime;
        //}
        //if(speed > DragDropObjects.WindPower / 100000)
        //{
        //    speed -= 1f * Time.deltaTime;
        //}
        speed = DragDropObjects.WindPower / 100000;
        PowerText.text = ((DragDropObjects.WindPower / 100000).ToString("0.00") + " MW");

        speed = Mathf.Clamp(speed, 0f, speedMax);
        
    }
    

    private void CreateSpeedLabels() {
        int labelAmount = 10;
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        for (int i = 0; i <= labelAmount; i++) {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("PowerText").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(labelSpeedNormalized * speedMax).ToString();
            speedLabelTransform.Find("PowerText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }

        needleTranform.SetAsLastSibling();
    }

    private float GetSpeedRotation() {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float speedNormalized = speed / speedMax;

        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}
