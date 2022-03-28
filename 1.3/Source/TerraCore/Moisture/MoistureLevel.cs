using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerraCore
{
	public static class MoistureLevel
	{
		public static readonly float DiffThresholdMinMax = 0.01f;

		public static readonly float None = 0f;

		public static readonly float Min = 0.0001f;

		public static readonly float Max = 0.9999f;

		public static readonly float Unreached = 1f;

		public static readonly float BaseParched = 0.2f;

		public static readonly float BaseDry = 0.4f;

		public static readonly float BaseNormal = 0.6f;

		public static readonly float BaseWet = 0.8f;

		public static readonly float ThresholdMin = None;

		public static readonly float ThresholdParchedDry = 0.3f;

		public static readonly float ThresholdDryNormal = 0.5f;

		public static readonly float ThresholdNormalWet = 0.7f;

		public static readonly float ThresholdMax = Unreached;

		public static readonly float ParchedMin = ThresholdMin;

		public static readonly float ParchedMax = ThresholdParchedDry + DiffThresholdMinMax;

		public static readonly float DryMin = ThresholdParchedDry - DiffThresholdMinMax;

		public static readonly float DryMax = ThresholdDryNormal + DiffThresholdMinMax;

		public static readonly float NormalMin = ThresholdDryNormal - DiffThresholdMinMax;

		public static readonly float NormalMax = ThresholdNormalWet + DiffThresholdMinMax;

		public static readonly float WetMin = ThresholdNormalWet - DiffThresholdMinMax;

		public static readonly float WetMax = ThresholdMax;

		public static float ThresholdEnumToFloat(MoistureLevelMinMaxEnum e)
		{
			switch (e)
			{
				case MoistureLevelMinMaxEnum.AlwaysMin:
					return None;
				case MoistureLevelMinMaxEnum.ParchedMin:
					return ParchedMin;
				case MoistureLevelMinMaxEnum.ParchedMax:
					return ParchedMax;
				case MoistureLevelMinMaxEnum.DryMin:
					return DryMin;
				case MoistureLevelMinMaxEnum.DryMax:
					return DryMax;
				case MoistureLevelMinMaxEnum.NormalMin:
					return NormalMin;
				case MoistureLevelMinMaxEnum.NormalMax:
					return NormalMax;
				case MoistureLevelMinMaxEnum.WetMin:
					return WetMin;
				case MoistureLevelMinMaxEnum.WetMax:
					return WetMax;
				case MoistureLevelMinMaxEnum.AlwaysMax:
					return Unreached;
				default:
					return None;
			}
		}

		public static float EnumToFloat(MoistureLevelEnum e)
		{
			switch (e)
			{
				case MoistureLevelEnum.Parched:
					return BaseParched;
				case MoistureLevelEnum.Dry:
					return BaseDry;
				case MoistureLevelEnum.Normal:
					return BaseNormal;
				case MoistureLevelEnum.Wet:
					return BaseWet;
				default:
					return None;
			}
		}
	}
}
