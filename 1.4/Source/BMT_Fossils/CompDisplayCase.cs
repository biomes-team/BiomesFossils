using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BMT_Fossils
{
	[StaticConstructorOnStartup]
	public class CompDisplayCase : ThingComp
    {
		public CompProperties_DisplayCase Props => (CompProperties_DisplayCase)props;

		public override void PostDraw()
		{
			Log.Message("Houston this is POST DRAW");
			base.PostDraw();
			//if (((Building_Storage)parent).GetSlotGroup().HeldThings.Count() > 0)
			//{
				Vector3 drawPos = parent.DrawPos;
				drawPos.y += 3f / 74f;

				Props.topGraphicData.Graphic.Draw(drawPos, Rot4.North, parent);
            
			//}
		}

	}
}
