/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
using System.IO.Pipes;
using TrackAndFieldResults.Seltec;

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Daten eines Athleten
    /// </summary>
    public partial class Athlete : IAthlete
    {
        public string Id { get ; set ; }
        public string WorldathleticsId { get ; set ; }
        public string NationalId { get ; set ; }
        public string Bib { get ; set ; }
        public int? YoB { get ; set ; }
        public string Firstname { get ; set ; }
        public Gender Gender { get ; set ; }
        public string Name { get ; set ; }
        public string Nationality { get ; set ; }
        public string Club { get ; set ; }

        public override string ToString()
        {
            return $"{System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name.ToLower())}, {Firstname}";
        }

        public static Athlete FromAthlete(Omega.Athlete athlete)
        {
            return new Athlete
            {
                Id = athlete.Id,
                WorldathleticsId = athlete.FedCode,
                Bib = athlete.Bib,
                Firstname = athlete.FirstName,
                Name = athlete.Name,
                Gender = athlete.Gender == "M" ? Common.Gender.Male : Common.Gender.Female,
                YoB = athlete.Age.ToYoB(),
                Nationality = athlete.Nationality,
                Club = athlete.Club,
            };
        }
        public static Athlete FromAthlete(Seltec.AthonCompetitor athlete, AthonClub club)
        {
            return new Athlete
            {
                Id= athlete.Id,
                WorldathleticsId = athlete._WA_Id,
                Bib = athlete.Bib,
                Firstname = athlete.Firstname,
                Name = athlete.Name,
                Gender = (Gender)athlete.Gender,
                YoB = athlete.YoB,
                Nationality = athlete.Nation,
                Club = club.Name,
            };
        }

    }

}