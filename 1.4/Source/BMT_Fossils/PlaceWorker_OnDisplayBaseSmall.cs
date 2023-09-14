using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_Fossils
{
    public class PlaceWorker_OnDisplayBaseSmall : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            Thing thing2 = map.thingGrid.ThingAt(loc, FossilsDefOf.BMT_DisplaySmall);
            if (thing2 == null || thing2.Position != loc)
            {
                return "BMT_PlaceOnDisplay".Translate();
            }
            return true;
        }


        public override bool ForceAllowPlaceOver(BuildableDef otherDef)
        {
            return otherDef == FossilsDefOf.BMT_DisplaySmall;
        }
    }
}
