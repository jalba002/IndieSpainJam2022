using CosmosDefender;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private PlayerAttributes playerAttributes;
    [SerializeField]
    private ShopModifiers shopModifiers;
    [SerializeField]
    private CanvasMenu[] menus;
    [SerializeField]
    private string gameLevel;

    private void Start()
    {
        //ActivateMenu(menus[0]);
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

    public void LoadGame()
    {
        playerAttributes.AddAttributeModifiers(shopModifiers.GetAttributeModifiers());
        playerAttributes.AddSpellModifiers(shopModifiers.GetSpellModifiers());
        SceneManager.LoadScene(gameLevel);
    }
}
