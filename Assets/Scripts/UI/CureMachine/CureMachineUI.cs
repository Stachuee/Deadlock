using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CureMachineUI : MonoBehaviour
{
    readonly int MAX_ITEMS_REQUIRED = 7;

    [SerializeField] GameObject cureMachine;
    [SerializeField] bool active;
    [SerializeField] Slider progressSlider;
    [SerializeField] GameObject noPower;

    [SerializeField] List<Slider> supports;
    [SerializeField] List<GameObject> supportsRequired;

    [SerializeField] Transform itemNeededContainder;
    List<Image> items;
    [SerializeField] GameObject itemRequiredPrefab;

    CureMachine cureMachineScript;

    int lastProgressIndex;
    CureController cureController;
    public void Open(bool on)
    {
        if (on) cureMachine.SetActive(true);
        else cureMachine.SetActive(false);
        active = on;
        noPower.SetActive(!CureMachine.Instance.IsPowered());
        UpdateItemsRequired();
    }

    private void Start()
    {
        cureController = CureController.instance;
        cureMachineScript = CureMachine.Instance;
        items = new List<Image>();

        for (int i = 0; i < MAX_ITEMS_REQUIRED; i++)
        {
            GameObject temp = Instantiate(itemRequiredPrefab);
            temp.transform.parent = itemNeededContainder;
            temp.transform.localScale = Vector3.one;
            temp.SetActive(false);
            items.Add(temp.GetComponent<Image>());
        }
    }

    private void Update()
    {
        if(active)
        {
            progressSlider.value = CureController.instance.GetCureCurrentProgress();
            for(int i = 0; i < supports.Count; i++)
            {
                supports[i].value = cureMachineScript.GetSupport(i);
            }

            if(lastProgressIndex != cureController.GetCurrentLevel())
            {
                UpdateItemsRequired();
            }
        }
    }

    public void UpdateItemsRequired()
    {
        lastProgressIndex = cureController.GetCurrentLevel();
        int id = 0;
        items.ForEach(item =>
        {
            item.gameObject.SetActive(false);
        });

        supportsRequired.ForEach(supportRequired =>
        {
            supportRequired.gameObject.SetActive(false);
        });

        CureMachine.Instance.GetRequiredSupport().ForEach(item =>
        {
            supportsRequired[(int)item].gameObject.SetActive(true);
        });

        CureMachine.Instance.GetRequiredItems().ForEach(item =>{
            items[id].sprite = item.GetIconSprite();
            items[id].gameObject.SetActive(true);
            id++;
        });
    }

}
