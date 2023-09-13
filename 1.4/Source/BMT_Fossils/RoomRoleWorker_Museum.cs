﻿using System;
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
            int num = 0;
            List<Thing> containedThings = room.ContainedAndAdjacentThings;

            for (int i = 0; i < containedThings.Count; i++)
            {
                Thing thing = containedThings[i];
                if (thing.HasThingCategory(ThingCategoryDef.Named("BMT_FossilCategory")))
                {
                    num++;
                }
            }

            return num * 5f;

        }
    }
}