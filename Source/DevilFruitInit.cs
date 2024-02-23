using RimWorld;
using Verse;

namespace DevilFruit;

[StaticConstructorOnStartup]
public static class DevilFruitInit
{
    static DevilFruitInit()
    {
        foreach (ThingDef def in DefDatabase<ThingDef>.AllDefsListForReading)
        {
            if (def.GetCompProperties<CompProperties_Neurotrainer>() is not { skill: not null }) continue;
            GraphicData graphicData = new();
            graphicData.CopyFrom(def.graphicData);
            graphicData.texPath = "Things/Item/Special/MechSerumSkilltrainer";
            def.graphicData = graphicData;
        }
    }
}
