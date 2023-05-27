using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUDController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] Slider hpSlider;
    [SerializeField] Image reload;

    private void Start()
    {
        if(playerController.isScientist) gameObject.SetActive(false);
    }

    private void Update()
    {
        hpSlider.maxValue = playerController.playerInfo.maxHp;
        hpSlider.value = playerController.playerInfo.hp;
    }

    public void UpdateReload(float progress)
    {
        reload.fillAmount = progress;
    }
}
