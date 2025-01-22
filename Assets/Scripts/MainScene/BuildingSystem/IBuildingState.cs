using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingState
{
    bool OnAction();
    void OnRotation();
    void UpdateState();
    void EndState();
}
