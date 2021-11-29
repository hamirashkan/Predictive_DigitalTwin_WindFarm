using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
    private BinarSave BS;
    private GameObject Predefined = null;

    // Start is called before the first frame update
    void Start()
    {
        Predefined = GameObject.Find("Predefined");

    }

    // Update is called once per frame
    void Update()
    {

    }
    [System.Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }

        public static implicit operator Vector3(SerializableVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        public static implicit operator SerializableVector3(Vector3 rValue)
        {
            return new SerializableVector3(rValue.x, rValue.y, rValue.z);
        }
    }

    [Serializable]
    public struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
        }

        public static implicit operator Quaternion(SerializableQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        public static implicit operator SerializableQuaternion(Quaternion rValue)
        {
            return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }

    [Serializable]
    public class BinarSave
    {
        public List<string> name = new List<string>();
        public List<string> tag = new List<string>();
        public List<SerializableVector3> position = new List<SerializableVector3>();
        public List<SerializableQuaternion> rotation = new List<SerializableQuaternion>();
        public List<SerializableVector3> scale = new List<SerializableVector3>();
        //public List<int> MaterialSize = new List<int>();
        //public List<string> MaterialNameList = new List<string>();

        //public List<string> barsname = new List<string>();
        //public List<SerializableVector3> barsposition = new List<SerializableVector3>();
    }

    public void DestroyChildren(string parentName)
    {
        Transform[] buildingSet = GameObject.Find(parentName).GetComponentsInChildren<Transform>();
        for (int i = 1; i < buildingSet.Length; i++)
        {
            Destroy(GameObject.Find(buildingSet[i].name) as GameObject);
        }
    }

    public void Fun_SaveScene()
    {

        BinaryFormatter BF = new BinaryFormatter();
        //FileStream Fs = File.Create(path);
        FileStream Fs = File.Create(Application.dataPath + "\\Save.txt");
        //FileStream Fs = File.Create(Application.persistentDataPath + fileName);
        //FileStream Fs = File.Create(System.IO.Directory.GetCurrentDirectory() + fileName);
        BS = null;
        BS = new BinarSave();

        Transform[] buildingSet = GameObject.Find("WindFarm").GetComponentsInChildren<Transform>();
        for (int i = 1; i < buildingSet.Length; i++)
        {
            if(buildingSet[i].transform.parent.name == "WindFarm")
            { 
                // customized properties
                BS.name.Add(buildingSet[i].name);
                BS.tag.Add(buildingSet[i].tag);
                BS.position.Add(buildingSet[i].position);
                BS.rotation.Add(buildingSet[i].rotation);
                BS.scale.Add(buildingSet[i].localScale);
            }
        }

        //MeshRenderer[] buildingMeshSet = GameObject.Find("WindFarm").GetComponentsInChildren<MeshRenderer>();
        //for (int i = 0; i < buildingMeshSet.Length; i++)
        //{

        //        BuildingAttributes objBA = (BuildingAttributes)GameObject.Find(buildingSet[i + 1].name).GetComponent(strBuildingAttributes);
        //        BS.MaterialSize.Add(objBA.MaterialNameList.Length);

        //        string materialNameList = "";
        //        for (int j = 0; j < objBA.MaterialNameList.Length; j++)
        //            materialNameList += objBA.MaterialNameList[j] + "#";
        //        if (materialNameList.Contains("#"))
        //            materialNameList = materialNameList.Substring(0, materialNameList.Length - 1);
        //        BS.MaterialNameList.Add(materialNameList);
        //}
        BF.Serialize(Fs, BS);
        Fs.Close();
    }
    public void Fun_LoadScene()
    {
        BinaryFormatter Bf = new BinaryFormatter();
        if (File.Exists(Application.dataPath + "\\Save.txt"))
        {
            FileStream FS = File.Open(Application.dataPath + "\\Save.txt", FileMode.Open);
            BinarSave bs = (BinarSave)Bf.Deserialize(FS);
            BS = bs;
            //DestroyChildren("WindFarm");
            for (int i = 0; i < bs.name.Count; i++)
            {
                GameObject obj = GameObject.Find(bs.name[i]);

                if (obj == null)
                {
                    Predefined.SetActive(true);
                    Transform[] PredefinedSet = Predefined.GetComponentsInChildren<Transform>();
                    // Filter the transform to only wind turbine and OilRig  used if(buildingSet[i].transform.parent.name == "Predefined")
                    for (int j = 1; j < PredefinedSet.Length; j++)
                        if (bs.name[i].Contains(PredefinedSet[j].name))
                        {
                            obj = Instantiate(GameObject.Find(PredefinedSet[j].name)) as GameObject;
                            obj.name = bs.name[i];
                            obj.tag = "Untagged";
                            obj.transform.parent = GameObject.Find("WindFarm").transform;
                        }
                    Predefined.SetActive(false);
                }

                if (obj != null)
                {
                    obj.transform.tag = bs.tag[i];
                    obj.transform.position = bs.position[i];
                    obj.transform.rotation = bs.rotation[i];
                    obj.transform.localScale = bs.scale[i];
                }
            }
            // HMI components, Gauage, Label
            //for (int i = 0; i < bs.barsname.Count; i++)
            //{
            //    GameObject obj = GameObject.Find(bs.barsname[i]);
            //    if (obj == null)
            //    {
            //        if (PredefinedBar != null)
            //        {
            //            PredefinedBar.SetActive(true);
            //            obj = Instantiate(PredefinedBar) as GameObject;
            //            obj.name = bs.barsname[i];
            //            obj.transform.parent = PredefinedBars.transform;
            //            PredefinedBar.SetActive(false);
            //        }
            //    }
            //    obj.transform.position = bs.barsposition[i];
            //}

            FS.Close();
        }
    }
}
