using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class SelectedTarget : MonoBehaviour
//{
//    private MeshRenderer render;

//    void Start()
//    {
//        render = GetComponent<MeshRenderer>();
//    }

//    private void OnMouseEnter()
//    {
//        render.material.color = Color.red;
//    }

//    private void OnMouseExit()
//    {
//        render.material.color = Color.white;
//    }

public class SelectedTarget : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    private Transform _selection;

    private void Update()
    {

        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material.color = Color.white;
            _selection = null;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                var SelectionRenderer = selection.GetComponent<Renderer>();
                if (SelectionRenderer != null)
                {
                    SelectionRenderer.material.color = highlightMaterial.color;
                }
            }
            _selection = selection;
        }
    }
}
