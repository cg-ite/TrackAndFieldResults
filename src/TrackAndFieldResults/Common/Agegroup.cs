/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Eine Altersklasse
    /// </summary>
    public partial class Agegroup : IAgegroup
    {
        public long Id { get ; set ; }
        public string Longname { get ; set ; }
        public Gender Gender { get ; set ; }
        public string Shortcode { get ; set ; }
        public int FromAge { get ; set ; }
        public int ToAge { get ; set ; }
        public int? Height { get ; set ; }
        public int? Weight { get ; set ; }
        public int? Distance { get ; set ; }
    }

    // macht so keinen Sinn, da height, weight, ... bei seltec immer
    // pro Event übertragen wird. Sinnvoll wäre func => getWeights(ageId)
    // wenn die gewichte nicht gesetzt sind


    /// <summary>
    /// Dictionary of the common german Agegroups, without
    /// the senior groups
    /// </summary>
    public class GerAgegroups
    {
        private static Agegroup[] all = new Agegroup[] {
            new Agegroup() {Id = 0,
                Longname = "weibliche Jugend U18",
                Gender = Gender.Female,
                Shortcode = "WJU18",
                FromAge = 16, ToAge = 17,
            },
            new Agegroup() {Id = 1,
                Longname = "weibliche Jugend U20",
                Gender = Gender.Female,
                Shortcode = "WJU20",
                FromAge = 18, ToAge = 19,
            },
            new Agegroup() {Id = 2,
                Longname = "weibliche U23",
                Gender = Gender.Female,
                Shortcode = "WU23",
                FromAge = 20, ToAge = 22,
            },
            new Agegroup() {Id = 3,
                Longname = "Frauen",
                Gender = Gender.Female,
                Shortcode = "W",
                FromAge = 23, ToAge = 29,
            },
            new Agegroup() {Id = 4,
                Longname = "männliche Jugend U18",
                Gender = Gender.Male,
                Shortcode = "MJU18",
                FromAge = 16, ToAge = 17,
            },
            new Agegroup() {Id = 5,
                Longname = "männliche Jugend U20",
                Gender = Gender.Male,
                Shortcode = "MJU20",
                FromAge = 18, ToAge = 19,
            },
            new Agegroup() {Id = 6,
                Longname = "männliche U23",
                Gender = Gender.Male,
                Shortcode = "MU23",
                FromAge = 20, ToAge = 22,
            },
            new Agegroup() {Id = 7,
                Longname = "Männer",
                Gender = Gender.Male,
                Shortcode = "M",
                FromAge = 23, ToAge = 29,
            }
            };
        public static Agegroup All(string shortcode)
        {
            return all.First(all => all.Shortcode == shortcode);
        }
    }
}
