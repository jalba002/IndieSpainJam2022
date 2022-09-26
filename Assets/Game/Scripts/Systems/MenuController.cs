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
    private string gameLevel;
    [SerializeField]
    private CosmosSpell basicAttack;

    public void LoadGame()
    {
        playerAttributes.RemoveAllSpells();
        playerAttributes.RemoveAllAttributeModifiers();
        playerAttributes.RemoveAllSpellModifiers();

        playerAttributes.AddSpell(basicAttack);
        playerAttributes.AddAttributeModifiers(shopModifiers.GetAttributeModifiers());
        playerAttributes.AddSpellModifiers(shopModifiers.GetSpellModifiers());
        SceneManager.LoadScene(gameLevel);
    }
}
