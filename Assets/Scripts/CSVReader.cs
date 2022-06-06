using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Globalization;
using System.Linq;

public class CSVReader : MonoBehaviour
{
    public TMP_Text dateText, tempText1, tempText2, tempText3, tempText4, tempText5, tempText6, tempText7, tempText8, tempText9, tempText10;
    public static float d1, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10,v1,v2,v3,v4;
    public int iter = 0;
    public static float minValue0,maxValue0, minValue1, maxValue1, minValue2, maxValue2, minValue3, maxValue3, minValue4, maxValue4, minValue5, maxValue5,
        minValue6, maxValue6, minValue7, maxValue7, minValue8, maxValue8, minValue9, maxValue9, minValue10, maxValue10, minValue11, maxValue11;
    public float SimulationSpeed;

    void Start()
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "python.exe";
        startInfo.Arguments = "-c import foo; print foo.hello()";
        process.StartInfo = startInfo;
        process.Start();
        //SimulationSpeed = 0.1f;
        //ReadCSVfile();
        readtext();
        readtext2();
        //readtext3();


    }

    public void getMinMax()
    {
        readtext2();
    }


    private void readtext()
    {
        StreamReader reader = new StreamReader(File.OpenRead(Application.dataPath + "/CSV/" + "BearingTemp_Vibr.csv"));
        List<string> DateList = new List<String>(); List<string> turbine1 = new List<String>(); List<string> turbine2 = new List<String>(); List<string> turbine3 = new List<String>();
        List<string> turbine4 = new List<String>(); List<string> turbine5 = new List<String>(); List<string> turbine6 = new List<String>(); List<string> turbine7 = new List<String>();
        List<string> turbine8 = new List<String>(); List<string> turbine9 = new List<String>(); List<string> turbine10 = new List<String>();
        List<string> vibration1 = new List<String>(); List<string> vibration2 = new List<String>(); 
        List<string> vibration3 = new List<String>(); List<string> vibration4 = new List<String>();

        //string vara1, vara2, vara3, vara4;
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (!String.IsNullOrWhiteSpace(line))
            {
                string[] values = line.Split(',');
                if (values.Length >= 10)
                {
                    DateList.Add(values[0]); turbine1.Add(values[1]); turbine2.Add(values[2]); turbine3.Add(values[3]);
                    turbine4.Add(values[4]); turbine5.Add(values[5]); turbine6.Add(values[6]); turbine7.Add(values[7]);
                    turbine8.Add(values[8]); turbine9.Add(values[9]); turbine10.Add(values[10]);
                    vibration1.Add(values[11]); vibration2.Add(values[12]); vibration3.Add(values[13]); vibration4.Add(values[14]);
                }
            }
        }
        string[] DateStr = DateList.ToArray(); string[] TurbineStr1 = turbine1.ToArray(); string[] TurbineStr2 = turbine2.ToArray(); string[] TurbineStr3 = turbine3.ToArray();
        string[] TurbineStr4 = turbine4.ToArray(); string[] TurbineStr5 = turbine5.ToArray(); string[] TurbineStr6 = turbine6.ToArray();
        string[] TurbineStr7 = turbine7.ToArray(); string[] TurbineStr8 = turbine8.ToArray(); string[] TurbineStr9 = turbine9.ToArray(); string[] TurbineStr10 = turbine10.ToArray();
        string[] vibrateStr1 = vibration1.ToArray(); string[] vibrateStr2 = vibration2.ToArray();
        string[] vibrateStr3 = vibration3.ToArray(); string[] vibrateStr4 = vibration4.ToArray();

        //List<float> yhat_lower = new List<float>();
        //List<float> yhat_upper = new List<float>();
        //for (int d = 1; d < TurbineStr1.Length; d++)
        //{
        //    var lower = (float.Parse)(TurbineStr1[d]);
        //    var upper = (float.Parse)(TurbineStr1[d]);
        //    yhat_lower.Add(lower);
        //}
        //Debug.Log("List:  " + yhat_lower);
        //Debug.Log("Maximum:  " + yhat_lower.Max() + "Minimum:  " + yhat_lower.Min());


        StartCoroutine(Simulate(SimulationSpeed, DateStr, TurbineStr1, TurbineStr2, TurbineStr3, TurbineStr4, TurbineStr5, TurbineStr6, TurbineStr7, TurbineStr8, TurbineStr9, TurbineStr10, vibrateStr1, vibrateStr2, vibrateStr3, vibrateStr4));

    }
    IEnumerator Simulate(float waitTime, string[] xdate, string[] x1, string[] x2, string[] x3, string[] x4, string[] x5, string[] x6, string[] x7, string[] x8, string[] x9, string[] x10, string[] y1, string[] y2, string[] y3, string[] y4)
    {
        while (true)
        {
            iter += 1;
            t1 = (float.Parse)(x1[iter]); t2 = (float.Parse)(x2[iter]);
            t3 = (float.Parse)(x3[iter]); t4 = (float.Parse)(x4[iter]);
            t5 = (float.Parse)(x5[iter]); t6 = (float.Parse)(x6[iter]);
            t7 = (float.Parse)(x7[iter]); t8 = (float.Parse)(x8[iter]);
            t9 = (float.Parse)(x9[iter]); t10 = (float.Parse)(x10[iter]);
            v1 = (float.Parse)(y1[iter]); v2 = (float.Parse)(y2[iter]);
            v3 = (float.Parse)(y3[iter]); v4 = (float.Parse)(y4[iter]);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void readtext2()
    {

        StreamReader reader_pred = new StreamReader(File.OpenRead(Application.dataPath + "/CSV/" + "Predicted.csv"));
        List<string> low0 = new List<String>(), high0 = new List<String>(), low1 = new List<String>(), high1 = new List<String>(), low2 = new List<String>(), high2 = new List<String>();
        List<string> low3 = new List<String>(), high3 = new List<String>(), low4 = new List<String>(), high4 = new List<String>(), low5 = new List<String>(), high5 = new List<String>();
        List<string> low6 = new List<String>(), high6 = new List<String>(), low7 = new List<String>(), high7 = new List<String>(), low8 = new List<String>(), high8 = new List<String>();
        List<string> low9 = new List<String>(), high9 = new List<String>(), low10 = new List<String>(), high10 = new List<String>(), low11 = new List<String>(), high11 = new List<String>();

        //string vara1, vara2, vara3, vara4;
        while (!reader_pred.EndOfStream)
        {
            string line = reader_pred.ReadLine();
            if (!String.IsNullOrWhiteSpace(line))
            {
                string[] values = line.Split(',');
                if (values.Length >= 10 & values != null)
                {
                    low0.Add(values[0]); high0.Add(values[1]); low1.Add(values[2]); high1.Add(values[3]); low2.Add(values[4]); high2.Add(values[5]);
                    low3.Add(values[6]); high3.Add(values[7]); low4.Add(values[8]); high4.Add(values[9]); low5.Add(values[10]); high5.Add(values[11]);
                    low6.Add(values[12]); high6.Add(values[13]); low7.Add(values[14]); high7.Add(values[15]); low8.Add(values[16]); high8.Add(values[17]);
                    low9.Add(values[18]); high9.Add(values[19]); low10.Add(values[20]); high10.Add(values[21]);
                }
            }
        }
        string[] lower0 = low0.ToArray(), higher0 = high0.ToArray(), lower1 = low1.ToArray(), higher1 = high1.ToArray(), lower2 = low2.ToArray(), higher2 = high2.ToArray(),
                lower3 = low3.ToArray(), higher3 = high3.ToArray(), lower4 = low4.ToArray(), higher4 = high4.ToArray(), lower5 = low5.ToArray(), higher5 = high5.ToArray(),
                lower6 = low6.ToArray(), higher6 = high6.ToArray(), lower7 = low7.ToArray(), higher7 = high7.ToArray(), lower8 = low8.ToArray(), higher8 = high8.ToArray(),
                lower9 = low9.ToArray(), higher9 = high9.ToArray(), lower10 = low10.ToArray(), higher10 = high10.ToArray();
        //Debug.Log("testttttt" + float.Parse(lower[1]));

        List<float> yhat_lower0 = new List<float>(), yhat_upper0 = new List<float>(), yhat_lower1 = new List<float>(), yhat_upper1 = new List<float>(),
            yhat_lower2 = new List<float>(), yhat_upper2 = new List<float>(), yhat_lower3 = new List<float>(), yhat_upper3 = new List<float>(),
            yhat_lower4 = new List<float>(), yhat_upper4 = new List<float>(), yhat_lower5 = new List<float>(), yhat_upper5 = new List<float>(),
            yhat_lower6 = new List<float>(), yhat_upper6 = new List<float>(), yhat_lower7 = new List<float>(), yhat_upper7 = new List<float>(),
            yhat_lower8 = new List<float>(), yhat_upper8 = new List<float>(), yhat_lower9 = new List<float>(), yhat_upper9 = new List<float>(),
            yhat_lower10 = new List<float>(), yhat_upper10 = new List<float>();
        for (int d = 1; d < lower0.Length; d++)
        {
            if (lower0[d] != "") { yhat_lower0.Add(float.Parse(lower0[d])); yhat_upper0.Add(float.Parse(higher0[d]));} else continue;
            if (lower1[d] != "") { yhat_lower1.Add(float.Parse(lower1[d])); yhat_upper1.Add(float.Parse(higher1[d])); } else continue;
            if (lower2[d] != "") { yhat_lower2.Add(float.Parse(lower2[d])); yhat_upper2.Add(float.Parse(higher2[d])); } else continue;
            if (lower3[d] != "") { yhat_lower3.Add(float.Parse(lower3[d])); yhat_upper3.Add(float.Parse(higher3[d])); } else continue;
            if (lower4[d] != "") { yhat_lower4.Add(float.Parse(lower4[d])); yhat_upper4.Add(float.Parse(higher4[d])); } else continue;
            if (lower5[d] != "") { yhat_lower5.Add(float.Parse(lower5[d])); yhat_upper5.Add(float.Parse(higher5[d])); } else continue;
            if (lower6[d] != "") { yhat_lower6.Add(float.Parse(lower6[d])); yhat_upper6.Add(float.Parse(higher6[d])); } else continue;
            if (lower7[d] != "") { yhat_lower7.Add(float.Parse(lower7[d])); yhat_upper7.Add(float.Parse(higher7[d])); } else continue;
            if (lower8[d] != "") { yhat_lower8.Add(float.Parse(lower8[d])); yhat_upper8.Add(float.Parse(higher8[d])); } else continue;
            if (lower9[d] != "") { yhat_lower9.Add(float.Parse(lower9[d])); yhat_upper9.Add(float.Parse(higher9[d])); } else continue;
            if (lower10[d] != "") { yhat_lower10.Add(float.Parse(lower10[d])); yhat_upper10.Add(float.Parse(higher10[d])); } else continue;
        }
        minValue0 = yhat_lower0.Min(); maxValue0 = yhat_upper0.Max(); minValue1 = yhat_lower1.Min(); maxValue1 = yhat_upper1.Max();
        minValue2 = yhat_lower2.Min(); maxValue2 = yhat_upper2.Max(); minValue3 = yhat_lower3.Min(); maxValue3 = yhat_upper3.Max();
        minValue4 = yhat_lower4.Min(); maxValue4 = yhat_upper4.Max(); minValue5 = yhat_lower5.Min(); maxValue5 = yhat_upper5.Max();
        minValue6 = yhat_lower6.Min(); maxValue6 = yhat_upper6.Max(); minValue7 = yhat_lower7.Min(); maxValue7 = yhat_upper7.Max();
        minValue8 = yhat_lower8.Min(); maxValue8 = yhat_upper8.Max(); minValue9 = yhat_lower9.Min(); maxValue9 = yhat_upper9.Max();
        minValue10 = yhat_lower10.Min(); maxValue10 = yhat_upper10.Max();
    }

    private void readtext3()
    {

        StreamReader reader_pred = new StreamReader(File.OpenRead(Application.dataPath + "/CSV/" + "rsmBoundary.csv"));
        List<string> low0 = new List<String>(), high0 = new List<String>(), low1 = new List<String>(), high1 = new List<String>(), low2 = new List<String>(), high2 = new List<String>();
        List<string> low3 = new List<String>(), high3 = new List<String>(), low4 = new List<String>(), high4 = new List<String>(), low5 = new List<String>(), high5 = new List<String>();
        List<string> low6 = new List<String>(), high6 = new List<String>(), low7 = new List<String>(), high7 = new List<String>(), low8 = new List<String>(), high8 = new List<String>();
        List<string> low9 = new List<String>(), high9 = new List<String>(), low10 = new List<String>(), high10 = new List<String>(), low11 = new List<String>(), high11 = new List<String>();

        //string vara1, vara2, vara3, vara4;
        while (!reader_pred.EndOfStream)
        {
            string line = reader_pred.ReadLine();
            if (!String.IsNullOrWhiteSpace(line))
            {
                string[] values = line.Split(',');
                if (values.Length >= 10 & values != null)
                {
                    low0.Add(values[0]); high0.Add(values[1]); low1.Add(values[2]); high1.Add(values[3]); low2.Add(values[4]); high2.Add(values[5]);
                    low3.Add(values[6]); high3.Add(values[7]); low4.Add(values[8]); high4.Add(values[9]); low5.Add(values[10]); high5.Add(values[11]);
                    low6.Add(values[12]); high6.Add(values[13]); low7.Add(values[14]); high7.Add(values[15]); low8.Add(values[16]); high8.Add(values[17]);
                    low9.Add(values[18]); high9.Add(values[19]); low10.Add(values[20]); high10.Add(values[21]);
                }
            }
        }
        string[] lower0 = low0.ToArray(), higher0 = high0.ToArray(), lower1 = low1.ToArray(), higher1 = high1.ToArray(), lower2 = low2.ToArray(), higher2 = high2.ToArray(),
                lower3 = low3.ToArray(), higher3 = high3.ToArray(), lower4 = low4.ToArray(), higher4 = high4.ToArray(), lower5 = low5.ToArray(), higher5 = high5.ToArray(),
                lower6 = low6.ToArray(), higher6 = high6.ToArray(), lower7 = low7.ToArray(), higher7 = high7.ToArray(), lower8 = low8.ToArray(), higher8 = high8.ToArray(),
                lower9 = low9.ToArray(), higher9 = high9.ToArray(), lower10 = low10.ToArray(), higher10 = high10.ToArray();
        //Debug.Log("testttttt" + float.Parse(lower[1]));

        List<float> yhat_lower0 = new List<float>(), yhat_upper0 = new List<float>(), yhat_lower1 = new List<float>(), yhat_upper1 = new List<float>(),
            yhat_lower2 = new List<float>(), yhat_upper2 = new List<float>(), yhat_lower3 = new List<float>(), yhat_upper3 = new List<float>(),
            yhat_lower4 = new List<float>(), yhat_upper4 = new List<float>(), yhat_lower5 = new List<float>(), yhat_upper5 = new List<float>(),
            yhat_lower6 = new List<float>(), yhat_upper6 = new List<float>(), yhat_lower7 = new List<float>(), yhat_upper7 = new List<float>(),
            yhat_lower8 = new List<float>(), yhat_upper8 = new List<float>(), yhat_lower9 = new List<float>(), yhat_upper9 = new List<float>(),
            yhat_lower10 = new List<float>(), yhat_upper10 = new List<float>();
        for (int d = 1; d < lower0.Length; d++)
        {
            if (lower0[d] != "") { yhat_lower0.Add(float.Parse(lower0[d])); yhat_upper0.Add(float.Parse(higher0[d])); } else continue;
            if (lower1[d] != "") { yhat_lower1.Add(float.Parse(lower1[d])); yhat_upper1.Add(float.Parse(higher1[d])); } else continue;
            if (lower2[d] != "") { yhat_lower2.Add(float.Parse(lower2[d])); yhat_upper2.Add(float.Parse(higher2[d])); } else continue;
            if (lower3[d] != "") { yhat_lower3.Add(float.Parse(lower3[d])); yhat_upper3.Add(float.Parse(higher3[d])); } else continue;
            if (lower4[d] != "") { yhat_lower4.Add(float.Parse(lower4[d])); yhat_upper4.Add(float.Parse(higher4[d])); } else continue;
            if (lower5[d] != "") { yhat_lower5.Add(float.Parse(lower5[d])); yhat_upper5.Add(float.Parse(higher5[d])); } else continue;
            if (lower6[d] != "") { yhat_lower6.Add(float.Parse(lower6[d])); yhat_upper6.Add(float.Parse(higher6[d])); } else continue;
            if (lower7[d] != "") { yhat_lower7.Add(float.Parse(lower7[d])); yhat_upper7.Add(float.Parse(higher7[d])); } else continue;
            if (lower8[d] != "") { yhat_lower8.Add(float.Parse(lower8[d])); yhat_upper8.Add(float.Parse(higher8[d])); } else continue;
            if (lower9[d] != "") { yhat_lower9.Add(float.Parse(lower9[d])); yhat_upper9.Add(float.Parse(higher9[d])); } else continue;
            if (lower10[d] != "") { yhat_lower10.Add(float.Parse(lower10[d])); yhat_upper10.Add(float.Parse(higher10[d])); } else continue;
        }
        minValue0 = yhat_lower0.Min(); maxValue0 = yhat_upper0.Max(); minValue1 = yhat_lower1.Min(); maxValue1 = yhat_upper1.Max();
        minValue2 = yhat_lower2.Min(); maxValue2 = yhat_upper2.Max(); minValue3 = yhat_lower3.Min(); maxValue3 = yhat_upper3.Max();
        minValue4 = yhat_lower4.Min(); maxValue4 = yhat_upper4.Max(); minValue5 = yhat_lower5.Min(); maxValue5 = yhat_upper5.Max();
        minValue6 = yhat_lower6.Min(); maxValue6 = yhat_upper6.Max(); minValue7 = yhat_lower7.Min(); maxValue7 = yhat_upper7.Max();
        minValue8 = yhat_lower8.Min(); maxValue8 = yhat_upper8.Max(); minValue9 = yhat_lower9.Min(); maxValue9 = yhat_upper9.Max();
        minValue10 = yhat_lower10.Min(); maxValue10 = yhat_upper10.Max();
    }
}
