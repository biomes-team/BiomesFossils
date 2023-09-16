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
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.Wait(job.def.joyDuration);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.tickAction = delegate
			{
				WaitTickAction();
			};
			toil.AddFinishAction(delegate
			{
				TryGainMuseumThought(pawn);
			});
			yield return toil;
		}


		private Thing MuseumThing => job.GetTarget(TargetIndex.A).Thing;
		protected void WaitTickAction()
		{
			float num = MuseumThing.GetStatValue(StatDefOf.Beauty) / MuseumThing.def.GetStatValueAbstract(StatDefOf.Beauty);
			float extraJoyGainFactor = ((num > 0f) ? num : 0f);
			pawn.GainComfortFromCellIfPossible();
			JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)MuseumThing);
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
