/*
 * SPDX - FileCopyrightText: Copyright © 2021 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */

namespace TrackAndFieldResults.Seltec
{
    /// <summary>
    /// Enum Klasse für die Shortcodes
    /// eines Events
    /// </summary>
    /// <remarks>Maybe not complete</remarks>
    public static class AthonShortcode
    {
        /// <summary>
        /// Stabhochsprung
        /// </summary>
        public const string Polevault = "PV";
        /// <summary>
        /// Weitsprung
        /// </summary>
        public const string Longjump = "LJ";
        /// <summary>
        /// Dreisprung
        /// </summary>
        public const string Triplejump = "TJ";
        /// <summary>
        /// Hochsprung
        /// </summary>
        public const string Highjump = "HJ";

        /// <summary>
        /// 110m Hürden
        /// </summary>
        public const string Hurdles110 = "11H";
        /// <summary>
        /// 100m Hürden
        /// </summary>
        public const string Hurdles100 = "10H";
        public const string Hurdles400 = "40H";
        /// <summary>
        /// 60m Hürden
        /// </summary>
        public const string Hurdles60 = "60H";

        /// <summary>
        /// 60m Sprint
        /// </summary>
        public const string Running60 = "60M";


        /// <summary>
        /// 100m Sprint
        /// </summary>
        public const string Running100 = "100";

        /// <summary>
        /// 200m Sprint
        /// </summary>
        public const string Running200 = "200";
        public const string Running300 = "300";

        /// <summary>
        /// 400m Sprint
        /// </summary>
        public const string Running400 = "400";
        /// <summary>
        /// 800m Mittelstrecke
        /// </summary>
        public const string Running800 = "800";
        public const string Running1000 = "1K0";

        /// <summary>
        /// 1500m Mittelstrecke
        /// </summary>
        public const string Running1500 = "1K5";
        public const string Running3000 = "3K0";
        public const string Running5000 = "5K0";
        public const string Running10000 = "10k";

        public const string Walking3000 = "3W";
        public const string Walking5000 = "5W";

        public const string Relay4x100 = "4X1";
        public const string Relay4x200 = "4X2";
        public const string Relay4x400 = "4X4";
        public const string Relay3x800 = "3X8";
        public const string Relay3x1000 = "3X1";
        /// <summary>
        /// Siebenkampf
        /// </summary>
        public const string Heptatlon = "HEP";
        /// <summary>
        /// Hallen 5-Kampf der Frauen
        /// </summary>
        public const string Indoor5k = "5HK";
        /// <summary>
        /// Hallen 7-Kampf der Männer
        /// </summary>
        public const string Indoor7k = "7-KH";
        /// <summary>
        /// Zehnkampf
        /// </summary>
        public const string Decathlon = "DEC";
        /// <summary>
        /// Kugelstoßen
        /// </summary>
        public const string Shortput = "SP";
        /// <summary>
        /// Speerwurf
        /// </summary>
        public const string JavelinThrow = "JT";
        /// <summary>
        /// Hammerwurf
        /// </summary>
        public const string HammerThrow = "HT";
        /// <summary>
        /// Diskuswurf
        /// </summary>
        public const string DiscusThrow = "DT";
    }
}
