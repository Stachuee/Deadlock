using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerActionEvent : UnityEvent<PlayerController>
{}

public interface IInteractable
{
    public void Use(PlayerController player);
    public void AddAction(UnityAction<PlayerController> action);
    public void RemoveAction(UnityAction<PlayerController> action);
    public void ClearActions();
}
