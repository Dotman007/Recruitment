using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public interface ICountry
    {
        void AddCountry(Country country);
        List<Country> GetAllCountries();
        void DeleteConfirm(int countryId);
        Country FindCountryById(int? id);
        void EditCountry(Country country);
        Country EditCountryById(int id);
    }
}
