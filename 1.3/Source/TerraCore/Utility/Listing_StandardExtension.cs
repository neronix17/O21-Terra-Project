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
	public static class Listing_StandardExtension
	{
		private const float BaseLineHeight = 30f;

		public static void GapGapLine(this Listing_Standard ls, float gapHeight = 12f)
		{
			ls.Gap(gapHeight);
			ls.GapLine(gapHeight);
			ls.Gap(gapHeight);
		}

		public static void CheckboxGroupLabeled(this Listing_Standard ls, string label, string tooltip, string cbLabel1, ref bool cbCheckOn1, string cbTooltip1, string cbLabel2, ref bool cbCheckOn2, string cbTooltip2)
		{
			TextAnchor anchor = Text.Anchor;
			Rect rect = ls.GetRect(30f);
			Rect rect2 = rect.LeftPart(0.5f).Rounded();
			Rect rect3 = rect.RightPart(0.5f).Rounded();
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			Text.Anchor = (TextAnchor)3;
			if (label != null)
			{
				Widgets.Label(rect2, label);
			}
			Text.Anchor = (TextAnchor)5;
			Rect rect4 = rect3.LeftPart(0.5f).RightPartPixels(Text.CalcSize(cbLabel1).x + 24f + 12f).Rounded();
			TooltipHandler.TipRegion(rect4, cbTooltip1);
			Widgets.CheckboxLabeled(rect4, cbLabel1, ref cbCheckOn1);
			Rect rect5 = rect3.RightPart(0.5f).RightPartPixels(Text.CalcSize(cbLabel2).x + 24f + 12f).Rounded();
			TooltipHandler.TipRegion(rect5, cbTooltip2);
			Widgets.CheckboxLabeled(rect5, cbLabel2, ref cbCheckOn2);
			Text.Anchor = anchor;
		}

		public static void TextFieldLabeled(this Listing_Standard ls, string label, ref string val, string tooltip = null)
		{
			TextAnchor anchor = Text.Anchor;
			Rect rect = ls.GetRect(30f);
			Rect rect2 = rect.LeftPart(0.5f).Rounded();
			Rect rect3 = rect.RightPart(0.5f).Rounded();
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			Text.Anchor = (TextAnchor)3;
			Widgets.Label(rect2, label);
			val = Widgets.TextField(rect3, val);
			Text.Anchor = anchor;
		}

		public static void IntegerFieldLabeled(this Listing_Standard ls, string label, ref int val, ref string buffer, string additionalInfoCol = null, string tooltip = null)
		{
			TextAnchor anchor = Text.Anchor;
			Rect rect = ls.GetRect(30f);
			Rect rect2 = rect.LeftPart(0.5f).Rounded();
			Rect rect3 = rect.RightPart((additionalInfoCol == null) ? 0.5f : 0.25f).Rounded();
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			Text.Anchor = (TextAnchor)3;
			Widgets.Label(rect2, label);
			if (additionalInfoCol != null)
			{
				Rect rect4 = rect.RightPart(0.5f).LeftPart(0.45f).Rounded();
				Text.Anchor = (TextAnchor)5;
				Widgets.Label(rect4, additionalInfoCol);
				Text.Anchor = (TextAnchor)3;
			}
			Widgets.TextFieldNumeric(rect3, ref val, ref buffer);
			Text.Anchor = anchor;
		}

		public static void SliderLabeled(this Listing_Standard ls, string label, ref float val, float min, float max, string format = null, string tooltip = null)
		{
			TextAnchor anchor = Text.Anchor;
			Rect rect = ls.GetRect(30f);
			Rect rect2 = rect.LeftHalf();
			Rect rect3 = rect.RightHalf();
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect2, tooltip);
			}
			Text.Anchor = (TextAnchor)3;
			Widgets.Label(rect2, label);
			string label2 = ((format != null) ? val.ToString(format) : val.ToString());
			val = Widgets.HorizontalSlider(rect3, val, min, max, middleAlignment: true, label2);
			Text.Anchor = anchor;
		}

		public static void SliderLabeled(this Listing_Standard ls, string label, ref int val, int min, int max, string format = null, string tooltip = null)
		{
			float val2 = val;
			SliderLabeled(ls, label, ref val2, min, max, format, tooltip);
			val = (int)val2;
		}
	}
}
