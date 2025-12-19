using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        Unit.ResetStaticData();
        BaseAction.ResetStaticData();
        SwordAction.ResetStaticData();
        ShootAction.ResetStaticData();
        DestructibleCrate.ResetStaticData();
        GrenadeProjectile.ResetStaticData();
    }
}
