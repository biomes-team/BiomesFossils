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
            List<IntVec3> cells = viewRect.Cells.ToList();

            //Log.Message(parent.def.defName + " has " + cells.Count() + " viewable cells centered at " + parent.Position.x + ", " + parent.Position.z);

            IntVec3 viewCell = new IntVec3(0,0,0);
            if(cells.Count >= 1)
            {
                cells = cells.Where(x => !PawnUtility.KnownDangerAt(x, pawn.Map, pawn) && !x.GetTerrain(pawn.Map).avoidWander && x.Standable(pawn.Map)).ToList();

            }
            viewCell = cells.RandomElement();

            //Log.Message("selected view cell: " + viewCell.x + ", " + viewCell.z);

            return viewCell;
        }

    }
}
