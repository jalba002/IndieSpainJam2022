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
        LaserEmpowered = Laser + Empowered,
        MeteorEmpowered = Meteor + Empowered,
        TornadoEmpowered = Tornado + Empowered,
        DashEmpowered = Dash + Empowered,
        Empowered = 1 << 4,
        All = ~(1 << 5)
    }
}