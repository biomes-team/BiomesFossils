using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace BMT_Fossils
{
    public class JobDriver_VisitMuseum : JobDriver
    {
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);

			Toil goToil = ToilMaker.MakeToil("GotoCell");
			goToil.initAction = delegate
            {
				goToil.actor.pather.StartPath(job.targetA.Thing.TryGetComp<CompDisplay>().GetViewCell(pawn), PathEndMode.OnCell);
			};
			goToil.defaultCompleteMode = ToilCompleteMode.PatherArrival;

			yield return goToil;

			Toil wait = ToilMaker.MakeToil("Wait");
			wait.initAction = delegate
			{
				wait.actor.pather.StopDead();
				wait.defaultCompleteMode = ToilCompleteMode.Delay;
				wait.defaultDuration = (int)Math.Max(500, 1000 * job.targetA.Thing.TryGetComp<CompDisplay>().displayValue);
			};
            wait.tickAction = delegate
            {
                WaitTickAction();
            };
            wait.AddFinishAction(delegate
            {
                TryGainMuseumThought(pawn);
            });
            yield return wait;

            Toil goToExhibitToil = ToilMaker.MakeToil("MakeNewToils");
			goToExhibitToil.initAction = delegate
			{
				if (job.targetQueueA.Count > 0)
				{
                    LocalTargetInfo targetA = job.targetQueueA[0];
                    job.targetQueueA.RemoveAt(0);
                    job.targetA = targetA;
                    JumpToToil(goToil);
                }
			};
			yield return goToExhibitToil;

		}


		private Thing MuseumThing => job.GetTarget(TargetIndex.A).Thing;
		
		protected void WaitTickAction()
		{
			float num = MuseumThing.TryGetComp<CompDisplay>().displayValue;
			//float num = MuseumThing.GetStatValue(StatDefOf.Beauty) / MuseumThing.def.GetStatValueAbstract(StatDefOf.Beauty);
			float extraJoyGainFactor = ((num > 0f) ? num : 0f);
			pawn.GainComfortFromCellIfPossible();

			//JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)MuseumThing);
			pawn.needs.joy?.GainJoy(extraJoyGainFactor *  0.36f / 2500f, FossilsDefOf.BMT_MuseumJoy);
            Need_Joy joy = pawn.needs.joy;
            if (joy != null && joy.CurLevel > 0.9999f)
            {
                pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
            }
        }


		public static void TryGainMuseumThought(Pawn pawn)
        {
			Room room = pawn.GetRoom();
			if (room != null)
			{
				if (room.Role == FossilsDefOf.BMT_Museum)
				{
					int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(FossilsDefOf.BMT_VisitedMuseum, scoreStageIndex));
					}
				}

				else
				{
					JoyUtility.TryGainRecRoomThought(pawn);
				}
				
			}
		}

	}
}
