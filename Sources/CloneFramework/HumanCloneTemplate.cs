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
    public class HumanCloneTemplate : ThingWithComps
    {
        public Pawn Source
        {
            get
            {
                CompHasPawnSources compHasPawnSources = this.GetComp<CompHasPawnSources>();
                return compHasPawnSources.pawnSources.FirstOrDefault();
            }
        }
        public void SetPawnSource(Pawn pawn)
        {
            CompHasPawnSources compHasPawnSources = this.GetComp<CompHasPawnSources>();
            if (compHasPawnSources != null)
            {
                compHasPawnSources.AddSource(pawn);
            }
        }
    }
}
