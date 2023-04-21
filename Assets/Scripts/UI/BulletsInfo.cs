using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsInfo : MonoBehaviour
{
    [SerializeField] Image bulletTypeSprite;
    [SerializeField] Text bulletsAmount;

    GunController gunController;


    [SerializeField] GameObject standartBullet;
    [SerializeField] GameObject fireBullet;
    [SerializeField] GameObject poisonBullet;

    private void Start()
    {
        gunController = FindObjectOfType<GunController>();
    }

    void Update()
    {
        if (gunController.GetCurrentGun().GetBulletType() == DamageType.Bullet) bulletTypeSprite.color = Color.gray;
        else if (gunController.GetCurrentGun().GetBulletType() == DamageType.Fire) bulletTypeSprite.color = Color.red;
        else if (gunController.GetCurrentGun().GetBulletType() == DamageType.Poison) bulletTypeSprite.color = Color.green;

        bulletsAmount.text = (gunController.GetCurrentGun().GetAmmoAmount()).ToString();
    }
}
