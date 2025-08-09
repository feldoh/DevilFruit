using RimWorld;
using Verse;

namespace DevilFruit.ConsumableGenepacks.HarmonyPatches;

public static class OneFruitOnlyPatch
{
    public static void AddGenelockedGene(Pawn ingester)
    {
        ingester.genes.AddGene(DevilFruitDefOf.DevilFruit_Eater, false);
    }

    // Dupe of AddGenelockedGene due to arg name difference
    public static void AddGenelockedGeneForUser(Pawn user) => AddGenelockedGene(user);

    public static bool AddGenePatch(ref Gene __result, Pawn_GeneTracker __instance, GeneDef geneDef, bool xenogene)
    {
        if (!DevilFruitMod.settings.oneFruitOnly || geneDef == DevilFruitDefOf.DevilFruit_Eater || !__instance.HasActiveGene(DevilFruitDefOf.DevilFruit_Eater)) return true;
        __result = null;
        Messages.Message("DevilFruit_Message_Blocked".Translate(geneDef.LabelCap), (Thing)__instance.pawn, MessageTypeDefOf.RejectInput);
        return false;
    }
}
