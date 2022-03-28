using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerraCore
{
	public static class FertilityLevel
	{
		public static readonly float DiffThresholdMinMax = 0.02f;

		public static readonly float None = 0f;

		public static readonly float Min = 0.0001f;

		public static readonly float Max = 1.9999f;

		public static readonly float Unreached = 2f;

		public static readonly float BaseSand = 0.1f;

		public static readonly float BaseLaterite = 0.45f;

		public static readonly float BaseBarren = 0.75f;

		public static readonly float BaseNormal = 1f;

		public static readonly float BaseFertile = 1.2f;

		public static readonly float BaseRich = 1.35f;

		public static readonly float ThresholdMin = None;

		public static readonly float ThresholdSandLaterite = 0.3f;

		public static readonly float ThresholdLateriteBarren = 0.6f;

		public static readonly float ThresholdBarrenNormal = 0.9f;

		public static readonly float ThresholdNormalFertile = 1.1f;

		public static readonly float ThresholdFertileRich = 1.25f;

		public static readonly float ThresholdMax = Unreached;

		public static readonly float SandMin = ThresholdMin;

		public static readonly float SandMax = ThresholdSandLaterite + DiffThresholdMinMax;

		public static readonly float LateriteMin = ThresholdSandLaterite - DiffThresholdMinMax;

		public static readonly float LateriteMax = ThresholdLateriteBarren + DiffThresholdMinMax;

		public static readonly float BarrenMin = ThresholdLateriteBarren - DiffThresholdMinMax;

		public static readonly float BarrenMax = ThresholdBarrenNormal + DiffThresholdMinMax;

		public static readonly float NormalMin = ThresholdBarrenNormal - DiffThresholdMinMax;

		public static readonly float NormalMax = ThresholdNormalFertile + DiffThresholdMinMax;

		public static readonly float FertileMin = ThresholdNormalFertile - DiffThresholdMinMax;

		public static readonly float FertileMax = ThresholdFertileRich + DiffThresholdMinMax;

		public static readonly float RichMin = ThresholdFertileRich - DiffThresholdMinMax;

		public static readonly float RichMax = ThresholdMax;

		public static float ThresholdEnumToFloat(FertilityLevelMinMaxEnum e)
		{
			switch (e)
			{
				case FertilityLevelMinMaxEnum.AlwaysMin:
					return None;
				case FertilityLevelMinMaxEnum.SandMin:
					return SandMin;
				case FertilityLevelMinMaxEnum.SandMax:
					return SandMax;
				case FertilityLevelMinMaxEnum.LateriteMin:
					return LateriteMin;
				case FertilityLevelMinMaxEnum.LateriteMax:
					return LateriteMax;
				case FertilityLevelMinMaxEnum.BarrenMin:
					return BarrenMin;
				case FertilityLevelMinMaxEnum.BarrenMax:
					return BarrenMax;
				case FertilityLevelMinMaxEnum.NormalMin:
					return NormalMin;
				case FertilityLevelMinMaxEnum.NormalMax:
					return NormalMax;
				case FertilityLevelMinMaxEnum.FertileMin:
					return FertileMin;
				case FertilityLevelMinMaxEnum.FertileMax:
					return FertileMax;
				case FertilityLevelMinMaxEnum.RichMin:
					return RichMin;
				case FertilityLevelMinMaxEnum.RichMax:
					return RichMax;
				case FertilityLevelMinMaxEnum.AlwaysMax:
					return Unreached;
				default:
					return None;
			}
		}

		public static float FertilityEnumToFloat(FertilityLevelEnum e)
		{
			switch (e)
			{
				case FertilityLevelEnum.Sand:
					return BaseSand;
				case FertilityLevelEnum.Laterite:
					return BaseLaterite;
				case FertilityLevelEnum.Barren:
					return BaseBarren;
				case FertilityLevelEnum.Normal:
					return BaseNormal;
				case FertilityLevelEnum.Fertile:
					return BaseFertile;
				case FertilityLevelEnum.Rich:
					return BaseRich;
				default:
					return None;
			}
		}
	}
}
