using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assignment.Management 
{
    public class UISettingsManager : MonoBehaviour
    {
        CameraManager cameraManagerScript;

        [SerializeField] GameObject currentlyDesigningText;

        [SerializeField] GameObject optionsPanel;
        bool panelIsOpen;
        bool designing;

        [SerializeField] GameObject colorPicker;
        Texture2D colorTexture;
        RectTransform rect;
        Color highlightColor = Color.yellow;

        [SerializeField] TMP_InputField scaleInputField;
        int scalePercentage = 50;

        [SerializeField] TMP_InputField rotationInputField;
        int degreesToTurn = 45;

        [SerializeField] TMP_InputField lightIntensityInputField;
        [SerializeField] Light pointLight;
        int lightIntensity = 5000;

        void Awake()
        {
            colorTexture = colorPicker.GetComponent<Image>().mainTexture as Texture2D;
            cameraManagerScript = FindObjectOfType<CameraManager>();
        }

        void Start()
        {
            rect = colorPicker.GetComponent<RectTransform>();
        }

        public void OpenSettingsPanel()
        {
            panelIsOpen = !panelIsOpen;
            optionsPanel.gameObject.SetActive(panelIsOpen);
        }

        public void DesignMode()
        {
            if (panelIsOpen) return;
            designing = !designing;
            currentlyDesigningText.SetActive(designing);
            cameraManagerScript.GetDesignCamera().gameObject.SetActive(designing);
        }

        public void SetHighlightColorSetting()
        {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, FindObjectOfType<InputManager>().GetMouseScreenPosition(), null, out delta);

            var width = rect.rect.width;
            var height = rect.rect.height;
            delta += new Vector2(width * .5f, height * .5f);

            var x = Mathf.Clamp(delta.x / width, 0f, 1f);
            var y = Mathf.Clamp(delta.y / height, 0f, 1f);

            var textureWidthX = Mathf.RoundToInt(x * colorTexture.width);
            var textureHeightY = Mathf.RoundToInt(y * colorTexture.height);

            var color = colorTexture.GetPixel(textureWidthX, textureHeightY);
            highlightColor = color;
        }

        public void ReadRotationInputSettings()
        {
            degreesToTurn = int.Parse(rotationInputField.GetComponent<TMP_InputField>().text);
        }

        public void ReadScaleInputSettings()
        {
            scalePercentage = int.Parse(scaleInputField.GetComponent<TMP_InputField>().text);
        }

        public void ReadAndSetLightIntensitySettings()
        {
            var lightIntensityMultiplier = 1000;
            lightIntensity = int.Parse(lightIntensityInputField.GetComponent<TMP_InputField>().text);
            pointLight.GetComponent<Light>().intensity = lightIntensity * lightIntensityMultiplier;
        }

        public int GetDegreesToTurn() => degreesToTurn;
        public int GetScalePercentage() => scalePercentage;
        public Color GetHighlightColor() => highlightColor;
        public Light GetPointLight() => pointLight;
        public bool GetDesignModeBool() => designing;
        public bool GetPanelIsOpenBool() => panelIsOpen;
    }
}
