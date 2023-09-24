using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace BMT_Fossils
{
    public class JobDriver_MuseumLearning : JobDriver
    {
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			this.FailOnChildLearningConditions();

			Toil goToil = ToilMaker.MakeToil("GotoCell");
			goToil.initAction = delegate
            {
				goToil.actor.pather.StartPath(job.targetA.Thing.TryGetComp<CompDisplay>().GetViewCell(pawn), PathEndMode.OnCell);
			};
			goToil.defaultCompleteMode = ToilCompleteMode.PatherArrival;

			yield return goToil;

            Toil wait = Toils_General.Wait(1000);
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
            pawn.needs.learning.Learn(.000015f * num);

			pawn.GainComfortFromCellIfPossible();
			if (pawn.needs.learning.CurLevel >= 0.999f)
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
			}
		}

	}
}
