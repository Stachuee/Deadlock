using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chute : InteractableBase
{
    [SerializeField]
    Chute connectedChute;

    [SerializeField]
    GameObject itemPrefab;

    [SerializeField]
    bool oneWay;
    [SerializeField]
    bool oneWayOutput;

    public static Chute scientistChute;

    protected override void Awake()
    {
        base.Awake();
        if(oneWayOutput)
        {
            if (scientistChute == null) scientistChute = this;
            else Debug.LogError("Two scientist chutes");
        }
        AddAction(DumpItems);
    }

    private void Start()
    {
        if(connectedChute == null) connectedChute = scientistChute;
    }

    void DumpItems(PlayerController player, UseType type)
    {
        if (oneWayOutput) return;
        for (ItemSO temp = player.DepositIngredient(); temp != null; temp = player.DepositIngredient())
        {
            connectedChute.DropItems(temp);
        }
        player.RefreshPrompt();
    }
    
    void DropItems(ItemSO item)
    {
        GameObject temp = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        temp.GetComponentInChildren<Item>().Innit(item);
    }

    public override bool IsUsable(PlayerController player)
    {
        return !oneWayOutput && player.CheckIfHoldingAny();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(connectedChute != null)Gizmos.DrawLine(transform.position, connectedChute.transform.position);
    }
}
