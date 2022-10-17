using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(SpellSpeedAdditiveModifier),
        menuName = "CosmosDefender/Modifiers/" + nameof(SpellSpeedAdditiveModifier))]
    public class SpellSpeedAdditiveModifier : BaseSpellModifier
    {
        [SerializeField] private float speedToAdd;

        public override void Modify(ref SpellData data)
        {
            data.Speed += speedToAdd;
        }

        public override float GetInitialValue(SpellData data)
        {
            return data.Speed;
        }

        public override float GetFinalValue(SpellData data)
        {
            return data.Speed += speedToAdd;
        }
    }
}