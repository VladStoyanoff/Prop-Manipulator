using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assignment.Management;

public class HighlightOnHover : MonoBehaviour
{

    [SerializeField] Material highlightMaterial;
    Transform highlightForProp;
    const int HIGHLIGHT_WIDTH = 18;
    const int CAN_RENDER_HIGHLIGHT_VARIABLE = 1;
    const int CANT_RENDER_HIGHLIGHT_VARIABLE = 0;
    const int COLOR_INTENSITY = 2;


    void OnMouseOver()
    {
        var visualOfProp = transform.GetChild(0);
        if (highlightForProp != null) return;
        highlightForProp = Instantiate(visualOfProp, gameObject.transform.position, visualOfProp.rotation, gameObject.transform);
        highlightForProp.localScale += visualOfProp.transform.localScale / HIGHLIGHT_WIDTH;
        var meshRenderer = highlightForProp.GetComponent<MeshRenderer>();
        meshRenderer.material = highlightMaterial;
        meshRenderer.material.color = FindObjectOfType<UISettingsManager>().GetHighlightColor() * COLOR_INTENSITY;
        visualOfProp.GetComponent<MeshRenderer>().rendererPriority = CAN_RENDER_HIGHLIGHT_VARIABLE;
    }

    void OnMouseExit()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().rendererPriority = CANT_RENDER_HIGHLIGHT_VARIABLE;
        Destroy(highlightForProp.gameObject);
    }
}