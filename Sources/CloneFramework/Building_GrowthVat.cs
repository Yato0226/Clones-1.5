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
using System.Reflection;

namespace CloneFramework
{
    [StaticConstructorOnStartup]
    public class Building_GrowthVatCloneFramework : Building_GrowthVat, IStoreSettingsParent, IThingHolderWithDrawnPawn, IThingHolder
    {
        private bool clone;

        private HumanCloneTemplate humanCloneTemplate;
        public HumanCloneTemplate HumanCloneTemplate
        {
            get
            {
                return humanCloneTemplate;
            }
            set
            {
                humanCloneTemplate = value;
            }
        }

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
            {
                yield return floatMenuOption;
            }

            if (!Working)
            {
                List<Thing> temps = base.Map.listerThings.ThingsOfDef(ThingDefOfLocal.HumanCloneTemplate);
                FloatMenuOption floatMenuOption = new FloatMenuOption("CloneFramework.ClonePawn".Translate(), delegate ()
                {
                    List<FloatMenuOption> list = new List<FloatMenuOption>();
                    foreach (HumanCloneTemplate temp in temps)
                    {
                        list.Add(new FloatMenuOption(temp.LabelCap, delegate ()
                        {
                            Job job = JobMaker.MakeJob(JobDefOfLocal.FillCloneTemplate, this, temp);
                            selPawn.jobs.TryTakeOrderedJob(job);
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(list));
                });
                if (!temps.NullOrEmpty())
                {
                    yield return floatMenuOption;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref clone, "clone");
            Scribe_Deep.Look(ref humanCloneTemplate, "humanCloneTemplate");
        }
    }
}
