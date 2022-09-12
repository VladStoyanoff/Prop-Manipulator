using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assignment.Management;

public class PropManipulator : MonoBehaviour
{
    InputManager inputManagerScript;
    UISettingsManager uiSettingsManagerScript;

    RaycastHit propHitByRay;
    [SerializeField] LayerMask propsLayerMask;

    [SerializeField] RectTransform contextMenuChair;
    [SerializeField] RectTransform contextMenuTable;
    [SerializeField] RectTransform contextMenuPicture;

    void Awake()
    {
        uiSettingsManagerScript = FindObjectOfType<UISettingsManager>();
        inputManagerScript = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        TryOpenContextMenu();
        TryMoveProp();
    }

    void TryOpenContextMenu()
    {
        if (inputManagerScript.RightMouseBtnIsPressedThisFrame() == false) return;
        if (Physics.Raycast(GetRay(), out RaycastHit hit, float.MaxValue, propsLayerMask) == false) return;
        if (uiSettingsManagerScript.GetPanelIsOpenBool()) return;
        if (uiSettingsManagerScript.GetDesignModeBool()) return;
        propHitByRay = hit;

        var propTag = propHitByRay.transform.tag;
        CloseAllContextMenus();

        if (propTag == "Chair")
        {
            HandleContextMenu(contextMenuChair);
        }

        if (propTag == "Table")
        {
            HandleContextMenu(contextMenuTable);
        }

        if (propTag == "Picture")
        {
            HandleContextMenu(contextMenuPicture);
        }
    }

    void HandleContextMenu(RectTransform contextMenu)
    {
        contextMenu.gameObject.SetActive(true);
        contextMenu.anchoredPosition = inputManagerScript.GetMouseScreenPosition();
    }

    void TryMoveProp()
    {
        if (inputManagerScript.LeftMouseBtnIsBeingHeld() == false) return;
        if (uiSettingsManagerScript.GetPanelIsOpenBool()) return;

        RaycastHit[] hits = Physics.RaycastAll(GetRay());
        if (hits.Length < 2) return;

        if (uiSettingsManagerScript.GetDesignModeBool() == false)
        {
            if (GameObject.FindGameObjectsWithTag("ContextMenu").Length != 0) return;
            Debug.Log("You can move props only in Design Mode.");
            return;
        }

        var rayHasHitPicture = hits[0].transform.gameObject.tag == "Picture";
        var positionRayLandedOnPlane = hits[1].point;
        var propHitByRayPosition = hits[0].transform;
        var offsetForPicture = new Vector3(0, .7f, -.8f);
        var offsetForOtherProps = new Vector3(0, 0, -.8f);

        propHitByRayPosition.position = positionRayLandedOnPlane;
        propHitByRayPosition.position += rayHasHitPicture ? offsetForPicture : offsetForOtherProps;
    }
    
    Ray GetRay() => Camera.main.ScreenPointToRay(inputManagerScript.GetMouseScreenPosition());

    #region Options for manipulating the chair

    public void Rotate(string direction)
    {
        CloseAllContextMenus();

        if (direction == "clockwise")
        {
            propHitByRay.transform.eulerAngles += GetDegrees();
        }

        else if (direction == "anti-clockwise")
        {
            propHitByRay.transform.eulerAngles -= GetDegrees();
        }

        else
        {
            Debug.LogError("No direction has been set. Use clockwise or anti-clockwise direction keywords");
        }
    }

    Vector3 GetDegrees() => new Vector3(0, uiSettingsManagerScript.GetDegreesToTurn(), 0);

    #endregion

    #region Options for manipulating the table

    public void Scale(string scaleString)
    {
        CloseAllContextMenus();

        if (scaleString == "grow")
        {
            propHitByRay.transform.localScale += GetScaleAmount();
        }

        else if (scaleString == "shrink")
        {
            propHitByRay.transform.localScale -= GetScaleAmount();
        }

        else
        {
            Debug.LogError("Scale settings is not set up. Use grow or shrink Scale keywords");
        }
    }

    Vector3 GetScaleAmount() => (float)uiSettingsManagerScript.GetScalePercentage() / 100 * propHitByRay.transform.localScale;

    #endregion

    #region Options for the picture

    public void SwitchLight(string switchedString)
    {
        CloseAllContextMenus();

        if (switchedString == "on")
        {
            uiSettingsManagerScript.GetPointLight().gameObject.SetActive(true);
        }

        else if (switchedString == "off")
        {
            uiSettingsManagerScript.GetPointLight().gameObject.SetActive(false);
        }

        else
        {
            Debug.LogError("Light settings is not set up. Use on or off keywords");
        }
    }

    public void CloseAllContextMenus()
    {
        GameObject[] contextMenus;
        contextMenus = GameObject.FindGameObjectsWithTag("ContextMenu");
        foreach (var contextMenu in contextMenus)
        {
            contextMenu.gameObject.SetActive(false);
        }
    }

    #endregion
}
