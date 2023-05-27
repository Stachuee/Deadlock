using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputDetection : MonoBehaviour
{

    public static InputDetection Instance { get; private set; }

    [SerializeField] GameObject gamepadPlayer1;
    [SerializeField] GameObject gamepadPlayer2;

    [SerializeField] GameObject keyboardPlayer1;
    [SerializeField] GameObject keyboardPlayer2;

    [SerializeField] Text player1Role;
    [SerializeField] Text player2Role;

    private int currentDeviceIndex;
    private bool isPickingDeviceForPlayer1 = true;
    private bool isPickingDeviceForPlayer2 = false;
    private InputDevice deviceForPlayer1;

    private bool chooseTime = false;

    [System.Serializable]
    public struct NewDevice
    {
        public bool scientist;
        public string controlScheme;
        public int deviceIndex;
        public InputDevice device;
    }

    private Dictionary<int, InputDevice> devices = new Dictionary<int, InputDevice>();
    List<NewDevice> newDevices = new List<NewDevice>();

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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
        if (!chooseTime) return;

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
                    newDevices.Add(new NewDevice() { device = device, deviceIndex = deviceId, controlScheme = "Keyboard", scientist = true });
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
                    newDevices.Add(new NewDevice() { device = device, deviceIndex = deviceId, controlScheme = "Keyboard", scientist = false });
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
                newDevices.Add(new NewDevice() { device = device, deviceIndex = deviceId, controlScheme = "Gamepad", scientist = true });
                gamepadPlayer1.SetActive(true);
                currentDeviceIndex++;
                isPickingDeviceForPlayer1 = false;
                isPickingDeviceForPlayer2 = true;
            }
            else if (isPickingDeviceForPlayer2)
            {
                gamepadPlayer2.SetActive(true);
                newDevices.Add(new NewDevice() { device = device, deviceIndex = deviceId, controlScheme = "Gamepad", scientist = false });
                currentDeviceIndex++;
                isPickingDeviceForPlayer2 = false;
                enabled = false;
            }
        }
    }
    public List<NewDevice> GetAllDevices()
    {
        return newDevices;
    }

    public void SetChooseTime(bool value)
    {
        chooseTime = value;
    }


    public void Swap()
    {
        if (newDevices.Count != 2) return;
        var device1 = newDevices[0];
        device1.scientist = !device1.scientist;
        newDevices[0] = device1;
        if (device1.scientist) player1Role.text = "Scientist";
        else player1Role.text = "Soldier";

        var device2 = newDevices[1];
        device2.scientist = !device2.scientist;
        newDevices[1] = device2;
        if (device2.scientist) player2Role.text = "Scientist";
        else player2Role.text = "Soldier";
    }
}
