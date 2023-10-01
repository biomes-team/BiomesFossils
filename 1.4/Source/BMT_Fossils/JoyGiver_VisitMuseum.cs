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
				Room room;

				candidates.AddRange(pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Art).Where(delegate (Thing thing)
				{
					if (thing.Faction != Faction.OfPlayer || thing.IsForbidden(pawn) || (!allowedOutside && !thing.Position.Roofed(thing.Map)) || !pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None) || !thing.IsPoliticallyProper(pawn))
					{
						return false;
					}
					CompDisplay compDisplay = thing.TryGetComp<CompDisplay>();
					if (compDisplay == null)
					{
						return false;
					}
					if (!compDisplay.Props.canBeMuseumViewed)
					{
						return false;
					}
					room = thing.GetRoom();
					if (room == null)
					{
						return false;
					}
					if (compDisplay.GetViewCell(pawn) == new IntVec3(0,0,0))
                    {
						return false;
                    }
					return (room.Role == FossilsDefOf.BMT_Museum) ? true : false;
				}));
				if (!candidates.TryRandomElementByWeight((Thing target) => Mathf.Max(target.GetStatValue(StatDefOf.Beauty), 0.5f), out var result))
				{
					return null;
				}

				Job job = JobMaker.MakeJob(def.jobDef, result);

				room = result.GetRoom();

				// Should return a randomized list of viewable exhibits
				List<Thing> exhibits = room.ContainedAndAdjacentThings.Where(t => t.TryGetComp<CompDisplay>() != null).ToList();
				exhibits = exhibits.Where(e => e.TryGetComp<CompDisplay>().Props.canBeMuseumViewed).ToList();
				//exhibits = exhibits.Where(t => t.TryGetComp<CompDisplay>().Props.canBeMuseumViewed == true).OrderBy(t => Rand.Value).ToList();

				job.targetQueueA = new List<LocalTargetInfo>();
				Thing ex = result;
				Thing newEx = result;
				exhibits.Remove(ex);
				for (int i = 1; i < exhibits.Count; i++)
				{
					newEx = exhibits.RandomElementByWeight(e => (float)Math.Max(.1, 15 - Math.Sqrt(Math.Pow(e.Position.x - ex.Position.x, 2) + Math.Pow(e.Position.z - ex.Position.z, 2))));
					IntVec3 viewCell = newEx.TryGetComp<CompDisplay>().GetViewCell(pawn);
					if(viewCell != new IntVec3(0,0,0))
                    {
						job.targetQueueA.Add(newEx);
					}
					exhibits.Remove(newEx);
					ex = newEx;
				}
				//job.locomotionUrgency = LocomotionUrgency.Walk;

				return job;
			}
			finally
			{
				candidates.Clear();
			}
		}
	}
}
  