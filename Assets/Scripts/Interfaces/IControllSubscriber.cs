using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllSubscriberAim
{
    public void ForwardCommandAim(Vector2 controll, Vector2 controllSmooth);
}
public interface IControllSubscriberUse
{
    public void ForwardCommandUse();
}

public interface IControllSubscriberBack
{
    public void ForwardCommandBack();
}

public interface IControllSubscriberShoot
{
    public void ForwardCommandShoot(bool shooting);
}

public interface IControllSubscriberMove
{
    public void ForwardCommandMove(Vector2 controll, Vector2 controllSmooth);
}
