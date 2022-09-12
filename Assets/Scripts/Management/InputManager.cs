#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assignment.Management
{
    public class InputManager : MonoBehaviour
    {
        PlayerInputAction playerInputAction;

        void Awake()
        {
            playerInputAction = new PlayerInputAction();
            playerInputAction.Player.Enable();
        }

        public Vector2 GetCameraMoveVector()
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputAction.Player.CameraMovement.ReadValue<Vector2>();
#else
            var inputMoveDir = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) inputMoveDir.y += 1;
            if (Input.GetKey(KeyCode.S)) inputMoveDir.y -= 1;
            if (Input.GetKey(KeyCode.A)) inputMoveDir.x -= 1;
            if (Input.GetKey(KeyCode.D)) inputMoveDir.x += 1;
            return inputMoveDir;
#endif
        }

        public float GetCameraRotateAmount()
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputAction.Player.CameraRotate.ReadValue<float>();
#else
            float rotateAmount = 0f;

            if (Input.GetKey(KeyCode.Q)) rotateAmount = +1f;
            if (Input.GetKey(KeyCode.E)) rotateAmount = -1f;

            return rotateAmount;
#endif
        }

        public float GetCameraZoomAmount()
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputAction.Player.CameraZoom.ReadValue<float>();
#else
            float zoomAmount = 0f;
            if (Input.mouseScrollDelta.y > 0) zoomAmount = -1f;
            if (Input.mouseScrollDelta.y < 0) zoomAmount = +1f;
            return zoomAmount;
#endif
        }

        public Vector2 GetMouseScreenPosition()
        {
#if USE_NEW_INPUT_SYSTEM
            return Mouse.current.position.ReadValue();
#else
            return Input.mousePosition;
#endif
        }

        public bool RightMouseBtnIsPressedThisFrame()
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputAction.Player.RightClick.WasPressedThisFrame();
#else
            return Input.GetMouseButtonDown(1);
#endif
        }

        public bool LeftMouseBtnIsBeingHeld()
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputAction.Player.LeftClick.IsPressed();
#else
            return Input.GetMouseButton(0);
#endif
        }
    }
}