﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_Fossils
{
    public class PlaceWorker_OnDisplayBaseLarge : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            Thing dispThing = map.thingGrid.ThingAt(loc, FossilsDefOf.BMT_DisplayLarge);

            if (dispThing == null)
            {
                return "BMT_PlaceOnDisplay".Translate();
            }
            CellRect dispRect = GenAdj.OccupiedRect(dispThing);

            CellRect fossilRect = GenAdj.OccupiedRect(loc, rot, checkingDef.Size);

            if (dispRect != fossilRect)
            {
                return "BMT_PlaceOnDisplay".Translate();
            }

            return true;
        }


        public override bool ForceAllowPlaceOver(BuildableDef otherDef)
        {
            return otherDef == FossilsDefOf.BMT_DisplayLarge;
        }
    }
}
