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

namespace CloneFramework
{
    [StaticConstructorOnStartup]
    public static class Start
    {
        static Start()
        {
            var harmony = new Harmony("DimonSever000.CloneFramework");
            harmony.PatchAll();
        }
    }
}
