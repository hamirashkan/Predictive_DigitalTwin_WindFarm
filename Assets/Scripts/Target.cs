// Change the mesh color in response to mouse actions.

using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    private Renderer rend;
    public Material highlightMat;
    public Material DefaultMat;
    void Start()
    {
        rend = this.GetComponent<Renderer>();
    }

    // The mesh goes red when the mouse is over it...
    void OnMouseEnter()
    {
        rend.material = highlightMat;
    }

    // ...the red fades out to cyan as the mouse is held over...
    //void OnMouseOver()
    //{
    //    rend.material.color -= new Color(1, 0, 0) * Time.deltaTime;
    //}

    // ...and the mesh finally turns white when the mouse moves away.
    void OnMouseExit()
    {
        rend.material = DefaultMat;
    }
}