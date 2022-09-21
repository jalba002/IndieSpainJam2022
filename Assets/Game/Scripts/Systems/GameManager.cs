using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PillarsConfig pillarsConfig;

    void Awake()
    {
        pillarsConfig.ClearObserverList();
    }

}