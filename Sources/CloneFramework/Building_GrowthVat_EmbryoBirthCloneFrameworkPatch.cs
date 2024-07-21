using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse.AI;
using Verse.Sound;
using Verse;
using HarmonyLib;
using System.Reflection;

namespace CloneFramework
{
    [HarmonyPatch(typeof(Building_GrowthVat))]
    [HarmonyPatch("EmbryoBirth")]
    public class Building_GrowthVat_EmbryoBirthCloneFrameworkPatch
    {
        private static bool Prefix(ref Building_GrowthVat __instance)
        {
            if (__instance is Building_GrowthVatCloneFramework building_GrowthVatCloneFramework)
            {
                int startTick = (int)typeof(Building_GrowthVat).GetField("startTick", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(building_GrowthVatCloneFramework);
                if (building_GrowthVatCloneFramework.selectedEmbryo != null && building_GrowthVatCloneFramework.innerContainer.Contains(building_GrowthVatCloneFramework.selectedEmbryo) && startTick <= Find.TickManager.TicksGame)
                {
                    Precept_Ritual ritual = Faction.OfPlayer.ideos.PrimaryIdeo.GetPrecept(PreceptDefOf.ChildBirth) as Precept_Ritual;
                    Thing thing = PregnancyUtility.ApplyBirthOutcome(((RitualOutcomeEffectWorker_ChildBirth)RitualOutcomeEffectDefOf.ChildBirth.GetInstance()).GetOutcome(0.7f, null), 0.7f, ritual, building_GrowthVatCloneFramework.selectedEmbryo?.GeneSet?.GenesListForReading, building_GrowthVatCloneFramework.selectedEmbryo.Mother, building_GrowthVatCloneFramework, building_GrowthVatCloneFramework.selectedEmbryo.Father);
                    float embryoStarvation = (float)typeof(Building_GrowthVat).GetField("embryoStarvation", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(building_GrowthVatCloneFramework);
                    if (thing != null && embryoStarvation > 0f)
                    {
                        Pawn pawn = ((thing is Corpse corpse) ? corpse.InnerPawn : ((Pawn)thing));
                        Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BioStarvation, pawn);
                        hediff.Severity = Mathf.Lerp(0f, HediffDefOf.BioStarvation.maxSeverity, embryoStarvation);
                        pawn.health.AddHediff(hediff);

                        if (building_GrowthVatCloneFramework.HumanCloneTemplate != null)
                        {
                            Pawn parent = building_GrowthVatCloneFramework.HumanCloneTemplate.Source;

                            pawn.gender = parent.gender;

                            if (!parent.story.traits.allTraits.NullOrEmpty())
                            {
                                foreach (Trait trait in parent.story.traits.allTraits)
                                {
                                    pawn.story.traits.allTraits.Add(new Trait(trait.def, trait.Degree));
                                }
                            }

                            pawn.genes.ClearXenogenes();
                            if (!parent.genes.GenesListForReading.NullOrEmpty())
                            {
                                foreach (Gene gene in parent.genes.GenesListForReading)
                                {
                                    pawn.genes.AddGene(gene.def, true);
                                }
                            }

                            pawn.story.skinColorOverride = parent.story.skinColorOverride;
                            pawn.story.HairColor = parent.story.HairColor;
                            pawn.story.favoriteColor = parent.story.favoriteColor;
                            pawn.story.hairDef = parent.story.hairDef;
                            pawn.story.furDef = parent.story.furDef;
                            pawn.story.headType = parent.story.headType;
                            pawn.story.bodyType = parent.story.bodyType;

                            building_GrowthVatCloneFramework.HumanCloneTemplate = null;
                        }
                    }
                }
                return false;
            }
            
            return true;
        }
    }
}
