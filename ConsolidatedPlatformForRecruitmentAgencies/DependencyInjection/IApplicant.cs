using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public  interface IApplicant
    {
        void Register(Applicant applicant);
        List<Gender> ListOfGender();
        List<Country> ListOfCountry();
        List<State> ListOfState();
        List<State> GetStateByCountry(int id);
        void SendEmail(int id);
        string GenerateRegNo();

    }
}
