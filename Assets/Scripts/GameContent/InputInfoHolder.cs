using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputInfoHolder : MonoBehaviour
{
    public static InputInfoHolder Instance { get; private set; }
    [SerializeField]
    List<InputDetection.NewDevice> heldDevices;
    bool useGlasses;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SaveDevices(List<InputDetection.NewDevice> devices)
    {
        heldDevices = devices;
    }

    public List<InputDetection.NewDevice> GetDevices()
    {
        return heldDevices;
    }

    public bool UseGlasses()
    {
        return useGlasses;
    }

    public void SetGlasses(bool value)
    {
        useGlasses = value;
    }
}
