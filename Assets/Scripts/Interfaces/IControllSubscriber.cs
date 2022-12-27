using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllSubscriberMovment
{
    public void ForwardCommandMovment(Vector2 controll);
}
public interface IControllSubscriberUes
{
    public void ForwardCommandUse();
}

public interface IControllSubscriberBack
{
    public void ForwardCommandBack();
}
