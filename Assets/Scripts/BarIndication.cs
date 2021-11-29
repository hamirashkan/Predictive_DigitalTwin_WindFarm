using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarIndication : MonoBehaviour
{
    private GameObject Value;
    private GameObject Label;
    private GameObject Name;
    public string LabelText = "Bar1";
    public float BarValue = 60;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            GameObject childGameObject = child.gameObject;
            if (childGameObject.name == "Value")
                Value = childGameObject;
            else if (childGameObject.name == "Label")
                Label = childGameObject;
            else if (childGameObject.name == "Name")
                Name = childGameObject;
        }
        Name.GetComponentInParent<TextMesh>().text = name;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Label.GetComponentInParent<TextMesh>().text = LabelText + "\n" + BarValue.ToString("0.0") + "%";
        Value.GetComponentInParent<Transform>().transform.localScale = new Vector3(1, 10f * BarValue/100, 1);
        Value.GetComponentInParent<Transform>().position = transform.position + new Vector3(0, 10f * BarValue / 100 / 2, 0);
    }
}
