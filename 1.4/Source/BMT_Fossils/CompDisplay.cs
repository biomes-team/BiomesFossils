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

                //if (parent.GetComp<CompAffectedByFacilities>() != null)
                //{
                //    CompAffectedByFacilities linkComp = parent.GetComp<CompAffectedByFacilities>();
                //    List<Thing> linked = linkComp.LinkedFacilitiesListForReading;
                //    foreach(Thing t in linked)
                //    {
                //        val *= Props.linkedMultiplier;
                //    }
                //}

                return val;
            }
        }

        public IntVec3 GetViewCell(Pawn pawn)
        {
            CellRect viewRect = GenAdj.OccupiedRect(parent.Position, parent.Rotation, new IntVec2((int)(parent.def.size.x + 2 * Props.maxViewDistance), (int)(parent.def.size.z + 2 * Props.maxViewDistance)));
            
            //if (Props.viewFromFront)
            //{
            //    IntVec3 center = parent.Position;
            //    GenAdj.AdjustForRotation(center, new IntVec2(parent.def.size.x, (int)Props.maxViewDistance), parent.Rotation);
                
            //}
            
            
            List<IntVec3> cells = viewRect.Cells.Where(c => c.GetRoom(parent.Map) == parent.GetRoom()).ToList();
            cells = cells.Where(x => !PawnUtility.KnownDangerAt(x, pawn.Map, pawn) && !x.GetTerrain(pawn.Map).avoidWander && x.Standable(pawn.Map)).ToList();

            IntVec3 viewCell = new IntVec3(0,0,0);
            if(cells.Count >= 1)
            {
                viewCell = cells.RandomElement();
            }

            return viewCell;
        }

    }
}
