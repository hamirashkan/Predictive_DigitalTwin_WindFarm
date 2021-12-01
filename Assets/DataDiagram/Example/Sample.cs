using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sample : MonoBehaviour {

    List<GameObject> lineList = new List<GameObject>();

    private DD_DataDiagram m_DataDiagram;
    //private RectTransform DDrect;

    private bool m_IsContinueInput = false;
    private float m_Input = 0f;
    private float h = 0;
    public TMP_Text Figuretext;

    void AddALine() {

        if (null == m_DataDiagram)
            return;

        Color color = Color.HSVToRGB((h += 0.4f) > 1 ? (h - 1) : h, 0.8f, 1.0f);
        GameObject line = m_DataDiagram.AddLine(color.ToString(), color);
        line.GetComponent<DD_Lines>().enabled = true;
        if (null != line)
            lineList.Add(line);
    }

    // Use this for initialization
    void Start () {
         GameObject dd = GameObject.Find("DataDiagram");
        if(null == dd) {
            Debug.LogWarning("can not find a gameobject of DataDiagram");
            return;
        }
        m_DataDiagram = dd.GetComponent<DD_DataDiagram>();

        m_DataDiagram.PreDestroyLineEvent += (s, e) => { lineList.Remove(e.line); };

        AddALine();
    }

    // Update is called once per frame
    void Update () 
    {
        Figuretext.text = (TurbineSetting.Title_GUI + "\n" + (TurbineSetting.WindOutputValue / 100000).ToString("0.00" + " MW") + "\n" + TurbineSetting.RPMValue.ToString("0.00" + " RPM") 
            + "\n" + (TurbineSetting.WindDirectionValue).ToString("0.0" + " Degree") + "\n" + (TurbineSetting.BladeLengthValue).ToString("0" + " Meter")); 

    }

    [System.Obsolete]
    private void FixedUpdate() {

        m_Input += Time.deltaTime;
        ContinueInput(m_Input);
        
        //for (int i = 0; i < WindFarm_totalPower.transform.GetChildCount(); i++)
        //{
        //    TotalOutPut += WindFarm_totalPower.transform.GetChild(i).GetComponent<TurbineSetting>().WindOutPut;
        //    avgOutput = TotalOutPut / i;
        //}

        m_DataDiagram.InputPoint(lineList[0], new Vector2(1, TurbineSetting.WindOutputValue / 100000));// avgOutput / 1000000));//WindFarmFMU.RPM_FMU));
    }







    private void ContinueInput(float f) {

        if (null == m_DataDiagram)
            return;

        if (false == m_IsContinueInput)
            return;

        float d = 0f;
        foreach (GameObject l in lineList) {
            m_DataDiagram.InputPoint(l, new Vector2(0.1f,
                (Mathf.Sin(f + d) + 1f) * 2f));
            d += 1f;
        }
    }

    public void onButton() {

        if (null == m_DataDiagram)
            return;

        foreach (GameObject l in lineList) {
            m_DataDiagram.InputPoint(l, new Vector2(1, Random.value * 4f));
        }
    }

    public void OnAddLine() {
        AddALine();
    }

    public void OnRectChange() {

        if (null == m_DataDiagram)
            return;

        Rect rect = new Rect(Random.value * Screen.width, Random.value * Screen.height,
            Random.value * Screen.width / 2, Random.value * Screen.height / 2);

        m_DataDiagram.rect = rect;
    }

    public void OnContinueInput() {

        m_IsContinueInput = !m_IsContinueInput;

    }

}
