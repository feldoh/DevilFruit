using System.Globalization;
using RimWorld;
using UnityEngine;
using Verse;

namespace DevilFruit;

public class Settings : ModSettings
{
    public bool oneFruitOnly = false;
    public bool hideDescription = true;

    public void DoWindowContents(Rect wrect)
    {
        Listing_Standard options = new();
        options.Begin(wrect);
        options.CheckboxLabeled("DevilFruit_Settings_OneFruitOnly".Translate(), ref oneFruitOnly);

        bool cur = hideDescription;
        options.CheckboxLabeled("DevilFruit_Settings_HideDescription".Translate(), ref hideDescription);
        if (hideDescription != cur)
        {
            PatchGeneTabs(ThingDefOf.Genepack);
        }
        options.Gap();
        options.End();
    }

    public void PatchGeneTabs(ThingDef def)
    {
        if (def == null) return;
        if (hideDescription)
        {
            def.inspectorTabs?.Remove(typeof(ITab_Genes));
            def.inspectorTabsResolved?.RemoveAll(t => t is ITab_Genes);
        }
        else if (!def.inspectorTabs.Contains(typeof(ITab_Genes)))
        {
            def.inspectorTabs?.AddDistinct(typeof(ITab_Genes));
            def.inspectorTabsResolved?.AddDistinct(InspectTabManager.GetSharedInstance(typeof(ITab_Genes)));
        }
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref oneFruitOnly, "oneFruitOnly", false);
        Scribe_Values.Look(ref hideDescription, "hideDescription", true);
    }

}
