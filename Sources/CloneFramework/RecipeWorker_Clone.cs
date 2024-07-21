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

namespace CloneFramework
{
    public class RecipeWorker_Clone : Recipe_Surgery
    {
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            HumanCloneTemplate humanCloneTemplate = ThingMaker.MakeThing(ThingDefOfLocal.HumanCloneTemplate) as HumanCloneTemplate;
            CompHasPawnSources comp = humanCloneTemplate.GetComp<CompHasPawnSources>();
            comp.AddSource(pawn);

            GenSpawn.Spawn(humanCloneTemplate, pawn.Position, pawn.Map);
        }
    }
}
