using System;

namespace CosmosDefender
{
    [Flags]
    public enum SpellType
    {
        Laser = 1 << 0,
        Meteor = 1 << 1,
        Thunder = 1 << 2,
        Dash = 1 << 3,
        Buff = 1 << 4,
        Empowered = 1 << 5,
        LaserEmpowered = Laser + Empowered,
        MeteorEmpowered = Meteor + Empowered,
        ThunderEmpowered = Thunder + Empowered,
        DashEmpowered = Dash + Empowered,
        BuffEmpowered = Buff + Empowered,
        All = ~(1 << 6)
    }
}