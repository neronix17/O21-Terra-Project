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
	public static class WaterLevel
	{
		public static readonly float DiffThresholdMinMax = 0.01f;

		public static readonly float None = 0f;

		public static readonly float Min = 0f;

		public static readonly float Max = 8f;

		public static readonly float BaseWet = 0.01f;

		public static readonly float BaseFlooded = 0.1f;

		public static readonly float BaseShallow = 0.35f;

		public static readonly float BaseHipDeep = 0.7f;

		public static readonly float BaseChestDeep = 1.15f;

		public static readonly float BaseSloping = 1.7f;

		public static readonly float BaseDeep = 3f;

		public static readonly float ThresholdMin = None;

		public static readonly float ThresholdWetFlooded = 0.02f;

		public static readonly float ThresholdFloodedShallow = 0.2f;

		public static readonly float ThresholdShallowHipDeep = 0.5f;

		public static readonly float ThresholdHipDeepChestDeep = 0.9f;

		public static readonly float ThresholdChestDeepSloping = 1.4f;

		public static readonly float ThresholdSlopingDeep = 2f;

		public static readonly float ThresholdMax = Max;

		public static readonly float WetMin = ThresholdMin;

		public static readonly float WetMax = ThresholdWetFlooded + DiffThresholdMinMax;

		public static readonly float FloodedMin = ThresholdWetFlooded - DiffThresholdMinMax;

		public static readonly float FloodedMax = ThresholdFloodedShallow + DiffThresholdMinMax;

		public static readonly float ShallowMin = ThresholdFloodedShallow - DiffThresholdMinMax;

		public static readonly float ShallowMax = ThresholdShallowHipDeep + DiffThresholdMinMax;

		public static readonly float HipDeepMin = ThresholdShallowHipDeep - DiffThresholdMinMax;

		public static readonly float HipDeepMax = ThresholdHipDeepChestDeep + DiffThresholdMinMax;

		public static readonly float ChestDeepMin = ThresholdHipDeepChestDeep - DiffThresholdMinMax;

		public static readonly float ChestDeepMax = ThresholdChestDeepSloping + DiffThresholdMinMax;

		public static readonly float SlopingMin = ThresholdChestDeepSloping - DiffThresholdMinMax;

		public static readonly float SlopingMax = ThresholdSlopingDeep + DiffThresholdMinMax;

		public static readonly float DeepMin = ThresholdSlopingDeep - DiffThresholdMinMax;

		public static readonly float DeepMax = ThresholdMax;

		public static float ThresholdEnumToFloat(WaterLevelMinMaxEnum e)
		{
			switch (e)
			{
				case WaterLevelMinMaxEnum.AlwaysMin:
					return None;
				case WaterLevelMinMaxEnum.WetMin:
					return WetMin;
				case WaterLevelMinMaxEnum.WetMax:
					return WetMax;
				case WaterLevelMinMaxEnum.FloodedMin:
					return FloodedMin;
				case WaterLevelMinMaxEnum.FloodedMax:
					return FloodedMax;
				case WaterLevelMinMaxEnum.ShallowMin:
					return ShallowMin;
				case WaterLevelMinMaxEnum.ShallowMax:
					return ShallowMax;
				case WaterLevelMinMaxEnum.HipDeepMin:
					return HipDeepMin;
				case WaterLevelMinMaxEnum.HipDeepMax:
					return HipDeepMax;
				case WaterLevelMinMaxEnum.ChestDeepMin:
					return ChestDeepMin;
				case WaterLevelMinMaxEnum.ChestDeepMax:
					return ChestDeepMax;
				case WaterLevelMinMaxEnum.SlopingMin:
					return SlopingMin;
				case WaterLevelMinMaxEnum.SlopingMax:
					return SlopingMax;
				case WaterLevelMinMaxEnum.DeepMin:
					return DeepMin;
				case WaterLevelMinMaxEnum.DeepMax:
					return DeepMax;
				case WaterLevelMinMaxEnum.AlwaysMax:
					return Max;
				default:
					return None;
			}
		}

		public static float EnumToFloat(WaterLevelEnum e)
		{
			switch (e)
			{
				case WaterLevelEnum.Wet:
					return BaseWet;
				case WaterLevelEnum.Flooded:
					return BaseFlooded;
				case WaterLevelEnum.Shallow:
					return BaseShallow;
				case WaterLevelEnum.HipDeep:
					return BaseHipDeep;
				case WaterLevelEnum.ChestDeep:
					return BaseChestDeep;
				case WaterLevelEnum.Sloping:
					return BaseSloping;
				case WaterLevelEnum.Deep:
					return BaseDeep;
				default:
					return None;
			}
		}
	}
}
