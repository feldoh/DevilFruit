using System.Collections.Generic;
using DevilFruit.ConsumableGenepacks.HarmonyPatches;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace DevilFruit;

public class DevilFruitMod : Mod
{
    public static Settings settings;

    public DevilFruitMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<Settings>();
        Harmony harmony = new("feldoh.devilfruit");
        if (AccessTools.TypeByName("ConsumableGenepack.ConsumableGenepack") is { } ConsumableGenepackType)
        {
            harmony.Patch(AccessTools.Method(ConsumableGenepackType, "PostIngested"), null,
                new HarmonyMethod(typeof(OneFruitOnlyPatch), nameof(OneFruitOnlyPatch.AddGenelockedGene)));

            harmony.Patch(AccessTools.Method(typeof(Pawn_GeneTracker), nameof(Pawn_GeneTracker.AddGene), [typeof(GeneDef), typeof(bool)]),
                new HarmonyMethod(typeof(OneFruitOnlyPatch), nameof(OneFruitOnlyPatch.AddGenePatch)));
        }

        harmony.Patch(AccessTools.PropertyGetter(typeof(GeneSetHolderBase), nameof(GeneSetHolderBase.DescriptionDetailed)), null,
            new HarmonyMethod(typeof(DevilFruitMod), nameof(HideDescription)));
        harmony.Patch(AccessTools.Method(typeof(GeneSetHolderBase), "GetInspectString"), null,
            new HarmonyMethod(typeof(DevilFruitMod), nameof(HideDescription)));
        harmony.Patch(AccessTools.Method(typeof(GeneSetHolderBase), nameof(Genepack.GetGizmos)), null,
            new HarmonyMethod(typeof(DevilFruitMod), nameof(HideGeneGizmo)));
        settings.PatchGeneTabs(ThingDefOf.Genepack);
    }

    public static void HideDescription(ref string __result, GeneSetHolderBase __instance)
    {
        __result = settings.hideDescription ? "DevilFruit_MysteryDescription".Translate(__instance.LabelCap) : __result;
    }

    public static IEnumerable<Gizmo> HideGeneGizmo(IEnumerable<Gizmo> __result, GeneSetHolderBase __instance)
    {
        foreach (Gizmo gizmo in __result)
        {
            switch (gizmo)
            {
                case Command_Action ca when settings.hideDescription && ca.defaultLabel.StartsWith("InspectGenes".Translate()):
                    break;
                default:
                    yield return gizmo;
                    break;
            }
        }
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "DevilFruit_Settings_Name".Translate();
    }
}
