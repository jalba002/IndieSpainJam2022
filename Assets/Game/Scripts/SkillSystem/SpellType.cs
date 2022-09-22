using System;

namespace CosmosDefender
{
    [Flags]
    public enum SpellType
    {
        Laser = 1 << 0,
        Meteor = 1 << 1,
        Tornado = 1 << 2,
        Dash = 1 << 3,
        LaserEmpowered = 1 << 4,
        MeteorEmpowered = 1 << 5,
        TornadoEmpowered = 1 << 6,
        DashEmpowered = 1 << 7,
        All = ~(1 << 8)
    }
}