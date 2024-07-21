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
    public class JobDriver_FillCloneTemplate : JobDriver
    {
        private int FillDuration => 300;
        private Building_GrowthVatCloneFramework building => job.GetTarget(TargetIndex.A).Thing as Building_GrowthVatCloneFramework;
        private HumanCloneTemplate item => job.GetTarget(TargetIndex.B).Thing as HumanCloneTemplate;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Reserve(building, job, 1, -1, null, errorOnFailed))
            {
                return pawn.Reserve(item, job, 1, -1, null, errorOnFailed);
            }
            return false;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            //AddEndCondition(() => building.HumanCloneTemplate != null ? JobCondition.Succeeded : JobCondition.Ongoing);

            if (job.count <= 0)
            {
                job.count = 1;
            }

            Toil getNextIngredient = Toils_General.Label();
            yield return getNextIngredient;
            yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden(TargetIndex.B);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(FillDuration).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A)
                .FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch)
                .WithProgressBarToilDelay(TargetIndex.A);

            Toil toil = new Toil();
            toil.initAction = delegate
            {
                building.HumanCloneTemplate = item;
                HumanEmbryo humanEmbryo = ThingMaker.MakeThing(ThingDefOf.HumanEmbryo) as HumanEmbryo;
                CompHasPawnSources comp = humanEmbryo.GetComp<CompHasPawnSources>();
                comp.AddSource(item.Source);
                humanEmbryo.TryPopulateGenes();
                building.SelectEmbryo(humanEmbryo);
                building.innerContainer.TryAdd(humanEmbryo);
            };
            toil.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return toil;

            yield return Toils_Jump.JumpIfHaveTargetInQueue(TargetIndex.B, getNextIngredient);
        }
    }
}
