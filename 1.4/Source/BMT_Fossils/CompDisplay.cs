using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace BMT_Fossils
{
    public class CompDisplay : ThingComp
    {
        public bool canBeEnjoyedAsArt;
        public CompProperties_Display Props => (CompProperties_Display)props;

        public float displayValue
        {
            get
            {
                float val = Props.museumPoints;

                if (parent.GetComp<CompAffectedByFacilities>() != null)
                {
                    CompAffectedByFacilities linkComp = parent.GetComp<CompAffectedByFacilities>();
                    List<Thing> linked = linkComp.LinkedFacilitiesListForReading;
                    foreach(Thing t in linked)
                    {
                        val *= Props.linkedMultiplier;
                    }
                }



                return val;
            }
        }
    }
}
