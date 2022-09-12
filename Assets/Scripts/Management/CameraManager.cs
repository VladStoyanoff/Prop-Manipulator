using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

namespace Assignment.Management
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] Slider cameraMovementSlider;
        [SerializeField] Slider cameraRotationSlider;
        [SerializeField] Slider cameraZoomSlider;

        [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] CinemachineVirtualCamera previewCamera;
        [SerializeField] CinemachineVirtualCamera designCamera;

        [SerializeField] GameObject picture;

        float moveSpeed = 25f;

        Vector3 rotationVector;
        float rotationSpeed = 25f;

        Vector3 followOffset;
        float zoomSpeed = 2.5f;
        const int MIN_FOLLOW_Y_OFFSET = 2;
        const int MAX_FOLLOW_Y_OFFSET = 12;

        CinemachineTransposer cinemachineTransposer;
        InputManager inputManagerScript;
        PropManipulator propManipulatorScript;
        UISettingsManager uiSettingsManagerScript;

        void Awake()
        {
            cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            inputManagerScript = FindObjectOfType<InputManager>();
            propManipulatorScript = FindObjectOfType<PropManipulator>();
            uiSettingsManagerScript = FindObjectOfType<UISettingsManager>();
        }

        void Start()
        {
            followOffset = cinemachineTransposer.m_FollowOffset;
        }

        void Update()
        {
            if (uiSettingsManagerScript.GetDesignModeBool()) return;
            if (uiSettingsManagerScript.GetPanelIsOpenBool()) return;
            UpdateCameraMovement();
            UpdateCameraRotation();
            UpdateZoomCamera();
        }

        void UpdateCameraMovement()
        {
            var inputMoveDir = inputManagerScript.GetCameraMoveVector();
            var moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }

        void UpdateCameraRotation()
        {
            rotationVector = Vector3.zero;
            rotationVector.y = inputManagerScript.GetCameraRotateAmount();
            transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
        }

        void UpdateZoomCamera()
        {
            followOffset.y += inputManagerScript.GetCameraZoomAmount();
            followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
        }

        public void ChangeMovementSpeed()
        {
            moveSpeed = cameraMovementSlider.value;
        }

        public void ChangeRotationSpeed()
        {
            rotationSpeed = cameraRotationSlider.value;
        }

        public void ChangeZoomSpeed()
        {
            zoomSpeed = cameraZoomSlider.value;
        }

        public void DeactivatePreviewCamera()
        {
            previewCamera.gameObject.SetActive(false);
            uiSettingsManagerScript.GetPointLight().gameObject.SetActive(false);
            propManipulatorScript.CloseAllContextMenus();
        }

        public void ActivatePreviewCamera()
        {
            var cameraOffset = new Vector3(0, .3f, -1.5f);
            previewCamera.gameObject.SetActive(true);
            previewCamera.transform.position = picture.transform.position + cameraOffset;
            uiSettingsManagerScript.GetPointLight().gameObject.SetActive(true);
            propManipulatorScript.CloseAllContextMenus();

        }
        public CinemachineVirtualCamera GetDesignCamera() => designCamera;
    }
}