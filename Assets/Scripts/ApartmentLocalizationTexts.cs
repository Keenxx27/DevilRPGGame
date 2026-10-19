using System.Collections.Generic;

namespace RPG
{
    public static class ApartmentLocalizationTexts
    {
        public static Dictionary<string, string[]> Create()
        {
            return new Dictionary<string, string[]>
            {
                { "apartment.speaker.linChunbao", new[] { "林春宝", "Lin Chunbao", "林春宝" } },
                { "apartment.speaker.municipalOfficer", new[] { "行政人员", "Municipal Officer", "行政職員" } }
            };
        }
    }
}
