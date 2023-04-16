using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLocker : InteractableBase
{
    [SerializeField]
    float searchSpeed;

    [SerializeField]
    List<ItemSO> items;

    [SerializeField]
    protected GameObject itemPrefab;

    PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        AddAction(SearchDrawer);
    }

    void SearchDrawer(PlayerController player)
    {
        player.LockInAction(StopSearching);
        StartCoroutine("StartSearching");
        this.player = player;
    }

    void StopSearching()
    {
        StopCoroutine("StartSearching");
    }

    IEnumerator StartSearching()
    {   
        while(items.Count > 0)
        {
            yield return new WaitForSeconds(searchSpeed);
            GameObject temp = Instantiate(itemPrefab, (Vector2)transform.position, Quaternion.identity);
            temp.GetComponentInChildren<Item>().Innit(items[0]);
            items.RemoveAt(0);
        }
        player.UnlockInAnimation();
    }
}
