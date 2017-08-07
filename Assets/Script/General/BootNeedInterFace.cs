using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BootState
{
    NeedBoot,
    Complete
}

public interface BootNeedInterFace
{
    BootState bootState { get; }

    void InitialState();
}
