
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Squishy.Irc
{
	public static class IrcChatUtil
	{
		/// <summary>
		/// Ctrl + B in mIRC (\u0002)
		/// </summary>
		public const char BoldControlCodeChar = '\u0002';

		/// <summary>
		/// Ctrl + K in mIRC (\u0003)
		/// </summary>
		public const char ColorControlCodeChar = '\u0003';

		/// <summary>
		/// Ctrl + U in mIRC (\u001F)
		/// </summary>
		public const char UnderlineControlCodeChar = '\u001F';

		/// <summary>
		/// Ctrl + O in mIRC (\u000F)
		/// </summary>
		public const char ControlEndCodeChar = '\u000F';

        /// <summary>
        /// Ctrl + B in mIRC (\u0002)
        /// </summary>
        public const string BoldControlCode = "\u0002";

        /// <summary>
        /// Ctrl + K in mIRC (\u0003)
        /// </summary>
        public const string ColorControlCode = "\u0003";

        /// <summary>
        /// Ctrl + U in mIRC (\u001F)
        /// </summary>
        public const string UnderlineControlCode = "\u001F";

        /// <summary>
        /// Ctrl + O in mIRC (\u000F)
        /// </summary>
        public const string ControlEndCode = "\u000F";

		public static string Colorize(this string text, IrcColorCode color)
		{
			return ColorControlCode + (int)color + text + ColorControlCode;
		}

        public static string Underline(this string text)
        {
            return UnderlineControlCode + text + UnderlineControlCode;
        }

		public static string Bold(this string text)
		{
			return BoldControlCode + text + BoldControlCode;
		}

		public static string GetColor(IrcColorCode color)
		{
			return ColorControlCode + (int)color;
		}

		public static string GetRtfUnicodeEscapedString(this string s)
		{
			var sb = new StringBuilder();
			foreach (var c in s)
			{
				if (c >= '0' && c <= 'Z')
					sb.Append(c);
				else
					sb.Append("\\u" + (int)(c) + "?");
			}
			return sb.ToString();
		}

		public static Color ParseColor(string ircColorCodeString, Color defaultColor)
		{
			IrcColorCode colorCode;
			if (!Enum.TryParse(ircColorCodeString, out colorCode))
				return defaultColor;
			Color color;
			if (!IrcToFormsColorLookup.TryGetValue(colorCode, out color))
				return defaultColor;

			return color;
		}

		public static ConsoleColor ParseConsoleColor(string ircColorCodeString, ConsoleColor defaultColor)
		{
			IrcColorCode colorCode;
			if (!Enum.TryParse(ircColorCodeString, out colorCode))
				return defaultColor;
			ConsoleColor color;
			if (!IrcToConsoleColorLookup.TryGetValue(colorCode, out color))
				return defaultColor;

			return color;
		}

		public static Dictionary<IrcColorCode, Color> IrcToFormsColorLookup = new Dictionary<IrcColorCode, Color>
		                                                                      	{
		                                                                                     		{IrcColorCode.White, Color.White},
																									{IrcColorCode.Black, Color.Black},
																									{IrcColorCode.DarkBlue, Color.DarkBlue},
																									{IrcColorCode.DarkGreen, Color.DarkGreen},
																									{IrcColorCode.Red, Color.Red},
																									{IrcColorCode.DarkRed, Color.DarkRed},
																									{IrcColorCode.Purple, Color.Purple},
																									{IrcColorCode.Orange, Color.Orange},
																									{IrcColorCode.Yellow, Color.Yellow},
																									{IrcColorCode.Green, Color.Green},
																									{IrcColorCode.Turquoise, Color.Turquoise},
																									{IrcColorCode.Cyan, Color.Cyan},
																									{IrcColorCode.Blue, Color.Blue},
																									{IrcColorCode.Violet, Color.Violet},
																									{IrcColorCode.DarkGrey, Color.DarkGray},
																									{IrcColorCode.LightGrey, Color.LightGray},
		                                                                                     	};

		public static Dictionary<IrcColorCode, ConsoleColor> IrcToConsoleColorLookup = new Dictionary<IrcColorCode, ConsoleColor>
		                                                                               	{
		                                                                                     		{IrcColorCode.White, ConsoleColor.White},
																									{IrcColorCode.Black, ConsoleColor.Black},
																									{IrcColorCode.DarkBlue, ConsoleColor.DarkBlue},
																									{IrcColorCode.DarkGreen, ConsoleColor.DarkGreen},
																									{IrcColorCode.Red, ConsoleColor.Red},
																									{IrcColorCode.DarkRed, ConsoleColor.DarkRed},
																									{IrcColorCode.Purple, ConsoleColor.Magenta},
																									{IrcColorCode.Orange, ConsoleColor.DarkYellow},
																									{IrcColorCode.Yellow, ConsoleColor.Yellow},
																									{IrcColorCode.Green, ConsoleColor.Green},
																									{IrcColorCode.Turquoise, ConsoleColor.DarkCyan},
																									{IrcColorCode.Cyan, ConsoleColor.Cyan},
																									{IrcColorCode.Blue, ConsoleColor.Blue},
																									{IrcColorCode.Violet, ConsoleColor.DarkMagenta},
																									{IrcColorCode.DarkGrey, ConsoleColor.DarkGray},
																									{IrcColorCode.LightGrey, ConsoleColor.Gray},
		                                                                                     	};

		private static string GetAndSkipColorCode(string str, ref int index)
		{
			string ircColorCode;
			//Is the character after that part of the
			//color code?

			if ((str[index + 1] == '0' || str[index + 1] == '1') &&
			    str[index + 2] >= '0' && str[index + 2] <= '9')
			{
				//grab the color code, excluding the
				//ctrl code
				ircColorCode = str.Substring(index + 1, 2);
				//discard the ctrl character and color code
				index += 3;
			}
			else
			{
				//grab the color code, excluding the
				//ctrl code
				ircColorCode = str.Substring(index + 1, 1);
				//discard the ctrl character and color code
				index += 2;
			}

			return ircColorCode;
		}
	}
}
