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

    [SerializeField] AudioSource sendItemSFX;

    [SerializeField] Sprite open;
    [SerializeField] Sprite closed;

    SpriteRenderer myRenderer;

    protected override void Awake()
    {
        base.Awake();
        if(oneWayOutput)
        {
            if (scientistChute == null) scientistChute = this;
            else Debug.LogError("Two scientist chutes");
        }
        AddAction(DumpItems);
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(connectedChute == null) connectedChute = scientistChute;
    }

    void DumpItems(PlayerController player, UseType type)
    {
        if (oneWayOutput || type == UseType.Computer) return;
        for (ItemSO temp = player.DepositIngredient(); temp != null; temp = player.DepositIngredient())
        {
            connectedChute.DropItems(temp);
            sendItemSFX.Play();
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

    public override void Highlight(UseType useType)
    {
        base.Highlight(useType);
        if (useType == UseType.Hand) myRenderer.sprite = open;
    }

    public override void UnHighlight(UseType useType)
    {
        base.UnHighlight(useType);
        if (useType == UseType.Hand) myRenderer.sprite = closed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(connectedChute != null)Gizmos.DrawLine(transform.position, connectedChute.transform.position);
    }


}
