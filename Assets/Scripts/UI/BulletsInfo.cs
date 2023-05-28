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

}
