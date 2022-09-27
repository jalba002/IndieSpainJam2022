using UnityEngine;

namespace CosmosDefender
{
    public interface IBuffProvider
    {
        Sprite BuffSprite { get; }
        
        int Tier { get; }
    }
}