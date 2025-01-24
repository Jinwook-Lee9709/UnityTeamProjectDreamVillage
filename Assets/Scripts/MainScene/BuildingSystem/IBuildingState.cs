

public interface IBuildingState
{
    bool OnAction();
    void OnRotation();
    void OnRemove();
    void UpdateState();
    void EndState();
}
