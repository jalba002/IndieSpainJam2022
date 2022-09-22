using CosmosDefender;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private PlayerAttributes playerAttributes;
    [SerializeField]
    private ShopModifiers shopModifiers;
    [SerializeField]
    private CanvasMenu[] menus;

    private void Start()
    {
        ActivateMenu(menus[0]);
    }

    public void ActivateMenu(CanvasMenu menu)
    {
        foreach (var current in menus)
        {
            if (current == menu)
                current.Show();
            else
                current.Hide();
        }
    }
}
