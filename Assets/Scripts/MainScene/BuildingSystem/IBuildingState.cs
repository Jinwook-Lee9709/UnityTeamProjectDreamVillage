using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingState
{
    void OnAction();
    void UpdateState();
    void EndState();
}
