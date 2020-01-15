using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public interface IPasswordSettings
    {
        bool isValidLength();
        int LengthErrorMessage();
        
        
    }
}
