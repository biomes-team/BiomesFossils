using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace BMT_Fossils
{
	public class JoyGiver_VisitMuseum : JoyGiver
	{

		private static List<Thing> candidates = new List<Thing>();


		public override Job TryGiveJob(Pawn pawn)
		{
			bool allowedOutside = JoyUtility.EnjoyableOutsideNow(pawn);
			try
			{
				candidates.AddRange(pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Art).Where(delegate (Thing thing)
				{
					if (thing.Faction != Faction.OfPlayer || thing.IsForbidden(pawn) || (!allowedOutside && !thing.Position.Roofed(thing.Map)) || !pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None) || !thing.IsPoliticallyProper(pawn))
					{
						return false;
					}
					CompDisplay compDisplay = thing.TryGetComp<CompDisplay>();
					if (compDisplay == null)
					{
						//Log.Error("No CompDisplay on thing being considered for viewing: " + thing);
						return false;
					}
					if (!compDisplay.Props.canBeMuseumViewed)
					{
						return false;
					}
					Room room = thing.GetRoom();
					if (room == null)
					{
						return false;
					}
					return (room.Role == FossilsDefOf.BMT_Museum) ? true : false;
				}));
				if (!candidates.TryRandomElementByWeight((Thing target) => Mathf.Max(target.GetStatValue(StatDefOf.Beauty), 0.5f), out var result))
				{
					return null;
				}
				return JobMaker.MakeJob(def.jobDef, result);
			}
			finally
			{
				candidates.Clear();
			}
		}
	}
}
  