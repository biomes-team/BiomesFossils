using System.Collections.Generic;
using System.Linq;
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

                if (parent.TryGetComp<CompDisplayCase>() != null)
                {
                    Building_Storage displayCase = (Building_Storage)parent;
                    if(displayCase.slotGroup.HeldThings.Any())
                    {
                        val = 0;
                        foreach(Thing t in displayCase.slotGroup.HeldThings)
                        {
                            val += t.TryGetComp<CompDisplay>().Props.museumPoints;
                        }
                    }
                }


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

        public bool isMuseumViewable
        {
            get
            {
                bool viewable = Props.isDisplay;

                if(!viewable)
                {
                    if (parent.TryGetComp<CompDisplayCase>() != null)
                    {
                        Building_Storage displayCase = (Building_Storage)parent;
                        if (displayCase.slotGroup != null && displayCase.slotGroup.HeldThings != null)
                        {
                            foreach (Thing thing in displayCase.slotGroup.HeldThings)
                            {
                                if (thing.TryGetComp<CompDisplay>()?.Props?.canBeMuseumViewed == true)
                                {
                                    viewable = true;
                                    break; // Exit loop early if a viewable item is found
                                }
                            }
                        }
                    }
                }
                return viewable;
            }
        }



        public IntVec3 GetViewCell(Pawn pawn)
        {
            //CellRect viewRect = GenAdj.OccupiedRect(parent.Position, parent.Rotation, new IntVec2((int)(parent.def.size.x + 2 * Props.maxViewDistance), (int)(parent.def.size.z + 2 * Props.maxViewDistance)));
            CellRect viewRect = GenAdj.OccupiedRect(parent).ExpandedBy((int)Props.maxViewDistance);


            viewRect = viewRect.ClipInsideMap(parent.Map);
            if (Props.viewFromFront)
            {
                Rot4 rot = parent.Rotation;
                CellRect parentRect = GenAdj.OccupiedRect(parent);

                if (rot == Rot4.North)
                {
                    viewRect.minZ = parentRect.maxZ + 1;
                }
                else if (rot == Rot4.South)
                {
                    viewRect.maxZ = parentRect.minZ - 1;
                }
                else if (rot == Rot4.East)
                {
                    viewRect.minX = parentRect.maxX + 1;
                }
                else if (rot == Rot4.West)
                {
                    viewRect.maxX = parentRect.minX - 1;
                }
            }

            List<IntVec3> cells = viewRect.Cells.Where(c => c.GetRoom(parent.Map) == parent.GetRoom()).ToList();
            cells = cells.Where(x => !PawnUtility.KnownDangerAt(x, pawn.Map, pawn) && !x.GetTerrain(pawn.Map).avoidWander && x.Standable(pawn.Map)).ToList();
            cells = cells.Where(x => GenSight.LineOfSight(x, parent.Position, parent.Map)).ToList();
            IntVec3 viewCell = new IntVec3(0,0,0);
            if(cells.Count >= 1)
            {
                viewCell = cells.RandomElement();
            }

            return viewCell;
        }

    }
}
