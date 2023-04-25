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
    public bool IsRemote();
    public Sprite GetComputerIcon();
    public bool IsProximity();
    public void Highlight();
    public bool HideInComputer();
    public ComputerInfoContainer GetInfo();
    public void UnHighlight();
    public Transform GetTransform();

    public Vector2 GetPosition();

}
