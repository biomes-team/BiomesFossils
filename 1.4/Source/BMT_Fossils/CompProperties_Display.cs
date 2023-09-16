using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BMT_Fossils
{
    public class CompProperties_Display : CompProperties
    {
		public float museumPoints = 1f;

		//public bool canBeEnjoyedAsArt;
		public bool canBeMuseumViewed = true;
		public float maxViewDistance = 3f;
		public bool isDisplay = true;
		public bool isInfo = false;

		public float linkedMultiplier = 1.2f;
		public int maxSimlutaneous = 1;
		public float maxLinkDistance = 4f;



		public CompProperties_Display()
		{
			compClass = typeof(CompDisplay);
		}
	}
}
