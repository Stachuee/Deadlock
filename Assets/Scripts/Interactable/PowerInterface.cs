using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchType { Doors, Security, Printers, Lights }
public interface PowerInterface
{
    public void PowerOn(bool on);

}
