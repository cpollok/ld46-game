using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMachine
{
    float GetPositionOnRail();

    void PutOnRail(Rail rail);

    bool Finished();
}
