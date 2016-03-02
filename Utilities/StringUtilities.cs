﻿using ColossalFramework.UI;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RoadsUnited.Utilities
{
    public static class StringUtilities
    {
        public static Color ExtractColourFromTags(string text, Color defaultColour)
        {
            Regex colourExtraction = new Regex("(?:<color)(#[0-9a-fA-F]+?)(>.*)");
            string extractedTag = colourExtraction.Replace(text, "$1");

            if (extractedTag != null && extractedTag != text && extractedTag != "")
            {
                defaultColour = UIMarkupStyle.ParseColor(extractedTag, defaultColour);
            }

            return defaultColour;
        }

        public static string RemoveTags(string text)
        {
            Regex tagRemover = new Regex("(<\\/?color.*?>)");

            return tagRemover.Replace(text, "");
        }
    }
}
