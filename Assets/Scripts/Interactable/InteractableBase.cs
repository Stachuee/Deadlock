using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField]
    PlayerActionEvent onTrigger = new PlayerActionEvent();

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
}
