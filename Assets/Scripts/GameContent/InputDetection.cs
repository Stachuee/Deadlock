using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDetection : MonoBehaviour
{
    [SerializeField] GameObject gamepadPlayer1;
    [SerializeField] GameObject gamepadPlayer2;

    [SerializeField] GameObject keyboardPlayer1;
    [SerializeField] GameObject keyboardPlayer2;

    private int currentDeviceIndex;
    private bool isPickingDeviceForPlayer1 = true;
    private bool isPickingDeviceForPlayer2 = false;
    private InputDevice deviceForPlayer1;

    private Dictionary<int, InputDevice> devices = new Dictionary<int, InputDevice>();

    private void Start()
    {
        currentDeviceIndex = 0;
        deviceForPlayer1 = null;
    }

    private void OnEnable()
    {
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["AnyKey"].performed += OnAnyKeyPerformed;
    }

    private void OnDisable()
    {
        var playerInput = GetComponent<PlayerInput>();
        playerInput.actions["AnyKey"].performed -= OnAnyKeyPerformed;
    }

    private void OnAnyKeyPerformed(InputAction.CallbackContext context)
    {
        InputDevice device = context.control.device;
        int deviceId = device.deviceId;
        string deviceName = device.name;
        Debug.Log(deviceId);

        if (deviceForPlayer1 != null && deviceId == deviceForPlayer1.deviceId)
        {
            return;
        }

        if (device is Keyboard)
        {
            if (isPickingDeviceForPlayer1)
            {
                if (!devices.ContainsKey(deviceId))
                {
                    deviceForPlayer1 = device;
                    devices.Add(deviceId, device);
                    keyboardPlayer1.SetActive(true);
                    currentDeviceIndex++;
                    isPickingDeviceForPlayer1 = false;
                    isPickingDeviceForPlayer2 = true;
                }
            }
            else if (isPickingDeviceForPlayer2)
            {
                if (!devices.ContainsKey(deviceId))
                {
                    devices.Add(deviceId, device);
                    keyboardPlayer2.SetActive(true);
                    currentDeviceIndex++;
                    isPickingDeviceForPlayer2 = false;
                    enabled = false;
                }
            }
        }
        else if (device is Gamepad)
        {
            if (isPickingDeviceForPlayer1)
            {
                deviceForPlayer1 = device;
                gamepadPlayer1.SetActive(true);
                currentDeviceIndex++;
                isPickingDeviceForPlayer1 = false;
                isPickingDeviceForPlayer2 = true;
            }
            else if (isPickingDeviceForPlayer2)
            {
                gamepadPlayer2.SetActive(true);
                currentDeviceIndex++;
                isPickingDeviceForPlayer2 = false;
                enabled = false;
            }
        }
    }
}
