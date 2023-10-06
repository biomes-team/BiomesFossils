using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BMT_Fossils
{
    public class CompProperties_DisplayCase : CompProperties
    {
        public GraphicData topGraphicData;

        public CompProperties_DisplayCase()
        {
            compClass = typeof(CompDisplayCase);
        }

        public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
        {
            GhostUtility.GhostGraphicFor(topGraphicData.Graphic, thingDef, ghostCol).DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef);
        }
    }
}
