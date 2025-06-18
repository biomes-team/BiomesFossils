using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_Fossils
{
    public class RoomRoleWorker_Museum : RoomRoleWorker
    {
        public override float GetScore(Room room)
        {
            float num = 0;
            List<Thing> containedThings = room.ContainedAndAdjacentThings;

            for (int i = 0; i < containedThings.Count; i++)
            {
                Thing thing = containedThings[i];
                if(thing.TryGetComp<CompDisplay>() != null)
                {
                    num += thing.TryGetComp<CompDisplay>().Props.museumPoints;
                }
                
                //if (thing.HasThingCategory(ThingCategoryDef.Named("BMT_FossilCategory")))
                //{
                //    num++;
                //}
                //else if (thing.def.tradeTags != null)
                //{ 
                //    if (thing.def.tradeTags.Contains("BMT_FossilTrade"))
                //    {
                //        num++;
                //    }
                //}
            }

            return num * 10f;

        }
    }
}
