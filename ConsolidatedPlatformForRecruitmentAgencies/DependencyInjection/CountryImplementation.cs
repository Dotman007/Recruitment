using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.Models;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public class CountryImplementation : ICountry
    {
        private RecruitmentContext _recruitmentContext;
        public CountryImplementation(RecruitmentContext recruitmentContext)
        {
            _recruitmentContext = recruitmentContext;
        }
        public void AddCountry(Country country)
        {
            _recruitmentContext.Countries.Add(country);
            _recruitmentContext.SaveChanges();
        }

        public void DeleteConfirm(int countryId)
        {
            var country = _recruitmentContext.Countries.Where(c => c.CountryId == countryId).SingleOrDefault();
            if (country != null)
            {
                _recruitmentContext.Countries.Remove(country);
                _recruitmentContext.SaveChanges();
            }
        }

        public void EditCountry(Country country)
        {
            _recruitmentContext.Entry(country).State = System.Data.Entity.EntityState.Modified;
            _recruitmentContext.SaveChanges();
        }

        public Country EditCountryById(int id)
        {
            var country = _recruitmentContext.Countries.Where(c => c.CountryId == id).SingleOrDefault();
            return country;
        }

        public Country FindCountryById(int? id)
        {
            Country country = _recruitmentContext.Countries.Find(id);
            return country;
        }

        public List<Country> GetAllCountries()
        {
            var countries = _recruitmentContext.Countries.ToList();
            return countries;
        }
    }
}