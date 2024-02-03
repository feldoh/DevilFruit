using RimWorld;
using Verse;

namespace DevilFruit;

[DefOf]
public static class DevilFruitDefOf
{
    [MayRequireBiotech]
    public static GeneDef DevilFruit_Eater;

    static DevilFruitDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(DevilFruitDefOf));
}
