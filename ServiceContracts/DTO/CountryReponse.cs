using Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryReponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType()  != typeof(CountryReponse))
            {
                return false;
            }

            CountryReponse country_to_compare = obj as CountryReponse;

            return this.CountryId == country_to_compare.CountryId && 
                        this.CountryName == country_to_compare.CountryName;
        }
    }

    /// <summary>
    /// Extension cover from Country to CountryReponse
    /// </summary>
    public static class CountryExtensions
    {
        public static CountryReponse ToCountryReponse(this Country country)
        {
            return new CountryReponse { CountryId = country.CountryId, CountryName = country.CountryName };
        }
    }

}
