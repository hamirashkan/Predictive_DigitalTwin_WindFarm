using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWrite : MonoBehaviour {
    
    private List<string[]> csvRowData = new List<string[]>();
    private string[] rowDataTemp = new string[17];
    public float SimulationSpeed;
    float time;

    void Start()
    {

        rowDataTemp[0] = "Date";
        rowDataTemp[1] = "Value1"; rowDataTemp[2] = "Value2"; rowDataTemp[3] = "Value3"; rowDataTemp[4] = "Value4"; rowDataTemp[5] = "Value5";
        rowDataTemp[6] = "Value6"; rowDataTemp[7] = "Value7"; rowDataTemp[8] = "Value8"; rowDataTemp[9] = "Value9"; rowDataTemp[10] = "Value10";
        rowDataTemp[11] = "Value11"; rowDataTemp[12] = "Value12"; rowDataTemp[13] = "Vibr1"; rowDataTemp[14] = "Vibr2"; rowDataTemp[15] = "Vibr3"; rowDataTemp[16] = "Vibr4";

        //rowDataTemp[3] = "Value3";
        csvRowData.Add(rowDataTemp);
        time = 0f;

    }
    void Update () {
        SaveCSV();
    }
    
    void SaveCSV(){
        time = time + 1f * Time.deltaTime;

        if (time >= SimulationSpeed)
        {
            time = 0f;
            rowDataTemp = new string[17];
            rowDataTemp[0] = DateTime.Now.ToString(); // name'
            //rowDataTemp[0] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",CultureInfo.InvariantCulture);
            rowDataTemp[1] = CSVReader.t1.ToString(); rowDataTemp[2] = CSVReader.t2.ToString(); rowDataTemp[3] = CSVReader.t3.ToString(); rowDataTemp[4] = CSVReader.t4.ToString(); 
            rowDataTemp[5] = CSVReader.t5.ToString(); rowDataTemp[6] = CSVReader.t6.ToString(); rowDataTemp[7] = CSVReader.t7.ToString(); rowDataTemp[8] = CSVReader.t8.ToString(); 
            rowDataTemp[9] = CSVReader.t9.ToString(); rowDataTemp[10] = CSVReader.t10.ToString(); rowDataTemp[11] = OPC_UA.WindTemp_OPCUA.ToString();
            rowDataTemp[12] = TurbineSetting.WindOutputValue.ToString(); // Income
            rowDataTemp[13] = CSVReader.v1.ToString(); rowDataTemp[14] = CSVReader.v2.ToString(); rowDataTemp[15] = CSVReader.v3.ToString(); rowDataTemp[16] = CSVReader.v4.ToString();
            //rowDataTemp[3] = TurbineSetting.RPMValue.ToString(); // Income
            csvRowData.Add(rowDataTemp);
        }



        string[][] output = new string[csvRowData.Count][];

        for(int i = 0; i < output.Length; i++){
            output[i] = csvRowData[i];
        }

        int     length         = output.GetLength(0);
        string     delimiter     = ",";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = csvGetPath();
        //string filePath = Application.dataPath + "/CSV/" + "Saved_data.csv";

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    

    // Following method is used to retrive the relative path as device platform
    private string csvGetPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Saved_data.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
        #else
        return Application.dataPath +"/"+"Saved_data.csv";
        #endif
    }
}