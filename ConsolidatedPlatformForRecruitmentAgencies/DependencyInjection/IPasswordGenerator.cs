using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(PasswordGeneratorSettings _passwordGeneratorSettings);
        bool PasswordIsValid(PasswordGeneratorSettings settings, string password);
    }
}
