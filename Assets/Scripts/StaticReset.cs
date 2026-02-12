using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticReset
{
    public static void ResetAll()
    {
        BaseAction.ResetStaticData();

        Unit.ResetStaticData();

        ShootAction.ResetStaticData();

        SwordAction.ResetStaticData();

        DestructibleCrate.ResetStaticData();

        Door.ResetStaticData();

        GrenadeProjectile.ResetStaticData();

        SpeedPotion.ResetStaticData();

        Unit.ResetStaticData();

        // Cualquier otro que tenga eventos static
    }
}
