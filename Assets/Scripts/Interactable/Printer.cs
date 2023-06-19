using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : PoweredInteractable, IGetHandInfo
{

    [SerializeField] bool broken;
    [SerializeField] ItemSO toRepair;

    [SerializeField] bool readyToCollect;

    [SerializeField] float baseProduction;
    [SerializeField] float productionRemain;

    [SerializeField] ItemSO toPrint;

    [SerializeField] GameObject prefabToPrint;

    [SerializeField] GameObject craftingBar;
    [SerializeField] Transform mask;
    [SerializeField] Vector2 startBarPos;
    [SerializeField] Vector2 endBarPos;
    
    Animator animator;

    [SerializeField] SpriteRenderer display;
    [SerializeField] GameObject donePrint;
    [SerializeField] ParticleSystem particles;

    protected override void Awake()
    {
        base.Awake();
        AddAction(Collect);
        productionRemain = baseProduction;
        animator = GetComponent<Animator>();
        display.sprite = toPrint.GetIconSprite();
        donePrint.GetComponent<SpriteRenderer>().sprite = toPrint.GetDefaultSprite();
        donePrint.SetActive(false);
    }


    private void Update()
    {
        if(!broken && powered && !readyToCollect)
        {
            productionRemain -= Time.deltaTime;
            if(productionRemain < 0)
            {
                readyToCollect = true;
                animator.SetBool("Done", true);
                donePrint.SetActive(true);
                particles.Play();
            }
            craftingBar.transform.localPosition = Vector2.Lerp(startBarPos, endBarPos, 1 - (productionRemain / baseProduction));
        }
    }

    public void Collect(PlayerController player, UseType type)
    {
        if (type == UseType.Computer) return;
        if (readyToCollect)
        {
            Instantiate(prefabToPrint, transform.position + new Vector3(Random.Range(-0.1f, 0.1f), 0, 0), Quaternion.identity).GetComponentInChildren<Item>().Innit(toPrint);
            readyToCollect = false;
            productionRemain = baseProduction;
            animator.SetBool("Done", false);
            donePrint.SetActive(false);
        }
        if (broken)
        {
            ItemSO input = player.CheckIfHoldingAnyAndDeposit(toRepair);
            if(input != null)
            {
                broken = false;
                player.UpdateHighlight();
            }
        }
    }

    public override ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer { showProgress = true, progress = (1 - productionRemain/baseProduction), showCharge = true, charged = powered, name = displayName };
    }

    public HandInfoContainer GetHandInfo()
    {
        if (broken) return new HandInfoContainer {show = true, name = toRepair.GetItemName(), sprite = toRepair.GetIconSprite() };
        else return new HandInfoContainer { show = false };
    }

    public override bool IsUsable(PlayerController player)
    {
        return readyToCollect || (broken && player.CheckIfHoldingAny(toRepair));
    }
    public override void PowerOn(bool on, string sectorName)
    {
        base.PowerOn(on, sectorName);
        animator.SetBool("Printing", on);
    }
}
