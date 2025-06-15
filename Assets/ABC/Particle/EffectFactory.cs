using UnityEngine;

public static class EffectFactory
{
    public enum EffectTypes
    {
        MuzzleFlash,
        BulletOnSurface,
        BloodOnEnemyHit,
        BloodOnPlayerHit,
    }

    public static void CreateEffect(EffectTypes effectType, Vector3 position)
    {
        switch (effectType)
        {
            case EffectTypes.MuzzleFlash:
                break;
            case EffectTypes.BulletOnSurface:
                break;
            case EffectTypes.BloodOnEnemyHit:
                break;
            case EffectTypes.BloodOnPlayerHit:
                break;
        }
    }


}


