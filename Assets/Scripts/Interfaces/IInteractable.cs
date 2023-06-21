using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UseType {Hand, Computer}

[System.Serializable]
public class PlayerActionEvent : UnityEvent<PlayerController, UseType>
{}

public interface IInteractable
{
    public void Use(PlayerController player, UseType action);
    public void AddAction(UnityAction<PlayerController, UseType> action);
    public void RemoveAction(UnityAction<PlayerController, UseType> action);
    public void ClearActions();
    public bool IsRemote();
    public GameObject GetComputerIcon();
    public bool IsProximity();
    public bool IsUsable(PlayerController player);
    public void Highlight();
    public void UnHighlight();
    public bool HideInComputer();
    public ComputerInfoContainer GetInfo();
    
    public Transform GetTransform();

    public Vector2 GetPosition();

}
