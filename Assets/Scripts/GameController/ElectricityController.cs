using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityController : MonoBehaviour
{
    public static ElectricityController Instance { get; private set; }


    public static float overcharge;
    public static float maxOvercharge;
    public static int fusesActive;
    public static int maxFusesActive;

    [SerializeField] float _maxOvercharge;
    [SerializeField] float overchargeFalloff;
    [SerializeField] int _maxFusesActive;
    [SerializeField] float[] overchargePerAdditionaFuse = {1, 1.25f, 1.5f, 1.75f, 2f};

    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        maxOvercharge = _maxOvercharge;
        maxFusesActive = _maxFusesActive;
    }

    private void Update()
    {
        if(fusesActive > maxFusesActive)
        {
            overcharge += overchargePerAdditionaFuse[Mathf.Min((fusesActive - 1) - maxFusesActive, overchargePerAdditionaFuse.Length - 1)] * Time.deltaTime;
        }
        overcharge = Mathf.Clamp(overcharge - overchargeFalloff * Time.deltaTime, 0, maxOvercharge);
    }


}
