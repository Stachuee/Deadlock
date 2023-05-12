using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected string displayName;
    [SerializeField] protected Sprite computerIcon;
    [SerializeField] PlayerActionEvent onTrigger = new PlayerActionEvent();
    [SerializeField] protected bool remoteActivation;
    [SerializeField] protected bool handActivation = true;
    [SerializeField] protected bool hideInComputer;

    protected virtual void Awake()
    {
        transform.tag = "Interactable";
    }

    public void AddAction(UnityAction<PlayerController> action)
    {
        onTrigger.AddListener(action);
    }

    public void RemoveAction(UnityAction<PlayerController> action)
    {
        onTrigger.RemoveListener(action);
    }

    public void Use(PlayerController player)
    {
        onTrigger.Invoke(player);
    }

    public void ClearActions()
    {
        onTrigger.RemoveAllListeners();
    }

    public bool IsRemote()
    {
        return remoteActivation;
    }
    public bool IsProximity()
    {
        return handActivation;
    }

    public bool HideInComputer()
    {
        return hideInComputer;
    }

    public virtual void Highlight()
    {
        SpriteRenderer myRendererTempHighlight = transform.GetComponent<SpriteRenderer>();
        myRendererTempHighlight.color = new Color(myRendererTempHighlight.color.r, myRendererTempHighlight.color.g, myRendererTempHighlight.color.b, 0.5f);
    }

    public virtual void UnHighlight()
    {
        SpriteRenderer myRendererTempHighlight = transform.GetComponent<SpriteRenderer>();
        myRendererTempHighlight.color = new Color(myRendererTempHighlight.color.r, myRendererTempHighlight.color.g, myRendererTempHighlight.color.b, 1f);
    }

    public virtual ComputerInfoContainer GetInfo()
    {
        return new ComputerInfoContainer { name = displayName };
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Sprite GetComputerIcon()
    {
        return computerIcon;
    }

    public virtual bool IsUsable(PlayerController player)
    {
        return true;
    }
}
