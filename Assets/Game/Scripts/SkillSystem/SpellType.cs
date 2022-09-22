using System;

namespace CosmosDefender
{
    [Flags]
    public enum SpellType
    {
        Laser = 2 << 0,
        Meteor = 2 << 1,
        Tornado = 2 << 2,
        Dash = 2 << 3,
        LaserEmpowered = 2 << 4,
        MeteorEmpowered = 2 << 5,
        TornadoEmpowered = 2 << 6,
        DashEmpowered = 2 << 7,
        All = ~(2 << 8)
    }
}