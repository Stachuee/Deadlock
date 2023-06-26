using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected string displayName;
    [SerializeField] protected GameObject computerIcon;
    [SerializeField] PlayerActionEvent onTrigger = new PlayerActionEvent();
    [SerializeField] protected bool remoteActivation;
    [SerializeField] protected bool handActivation = true;
    [SerializeField] protected bool hideInComputer;

    [SerializeField] protected GameObject outline;
    protected virtual void Awake()
    {
        transform.tag = "Interactable";
    }

    public void AddAction(UnityAction<PlayerController, UseType> action)
    {
        onTrigger.AddListener(action);
    }

    public void RemoveAction(UnityAction<PlayerController, UseType> action)
    {
        onTrigger.RemoveListener(action);
    }

    public void Use(PlayerController player, UseType useType)
    {
        onTrigger.Invoke(player, useType);
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

    public virtual void Highlight(UseType useType)
    {
        if (useType == UseType.Hand)
        {
            outline.SetActive(true);
        }
    }

    public virtual void UnHighlight(UseType useType)
    {
        if (useType == UseType.Hand)
        {
            outline.SetActive(false);
        }
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

    public GameObject GetComputerIcon()
    {
        return computerIcon;
    }

    public virtual bool IsUsable(PlayerController player)
    {
        return true;
    }

}
