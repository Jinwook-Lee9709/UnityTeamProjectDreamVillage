using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlacementSystem placementSystem;
    public PlacementSystem PlacementSystem
    {
        get { return placementSystem; }
    }
}
