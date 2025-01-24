using AYellowpaper.SerializedCollections;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializedDictionary, SerializeField] private SerializedDictionary<MainSceneUiIds, GameObject> UiObjects = new ();
    
    public GameObject GetPanel(MainSceneUiIds id)
    {
        if (UiObjects.ContainsKey(id))
        {
            return UiObjects[id];
        }
        else
        {
            return null;
        }
    }
}
