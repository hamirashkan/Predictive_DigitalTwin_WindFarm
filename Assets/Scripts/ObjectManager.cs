using UnityEngine;
public class ObjectManager : MonoBehaviour
{
    Vector3 Dist;
    private float PosX;
    private float PosY;
    private float PosZ;
    bool shiftOn = true;
    private Vector3 default_pos, default_rot, editModePos, editModeRot;

    private void Awake()
    {
        default_pos = GameObject.Find("Main Camera").transform.position;
        default_rot = GameObject.Find("Main Camera").transform.eulerAngles;
        editModePos = new Vector3(-25.5032902f, 160, -26.4962082f);
        editModeRot = new Vector3(90, 0, 0);
    }
    void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    Debug.Log("Shift Pressed");
        //    shiftOn = false;
        //    GameObject.Find("Main Camera").transform.position = editModePos;
        //    GameObject.Find("Main Camera").transform.eulerAngles = editModeRot;
        //}

        //if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //    Debug.Log("Shift Released");
        //    shiftOn = true;
        //    GameObject.Find("Main Camera").transform.position = default_pos;
        //    GameObject.Find("Main Camera").transform.eulerAngles = default_rot;
        //}
    }

    private void OnMouseDown()
    {
        if (!shiftOn)
        {
            Dist = Camera.main.WorldToScreenPoint(this.transform.position);
            PosX = Input.mousePosition.x - Dist.x;
            PosY = Input.mousePosition.y - Dist.y;
            PosZ = Input.mousePosition.y - Dist.z;
        }
    }

    private void OnMouseDrag()
    {
        if (!shiftOn)
        {
            Vector3 CurrentPos = new Vector3(Input.mousePosition.x - PosX, Input.mousePosition.y - PosY, Dist.z);
            Vector3 WorldPos = Camera.main.ScreenToWorldPoint(CurrentPos);
            this.transform.parent.position = WorldPos;

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //RaycastHit hitInfo;
            //if (Physics.Raycast(ray, out hitInfo))
            //{
            //    this.transform.position = hitInfo.point;
            //    this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            //}
        }
    }
}