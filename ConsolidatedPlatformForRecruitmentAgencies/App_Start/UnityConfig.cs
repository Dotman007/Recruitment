using ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection;
using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ConsolidatedPlatformForRecruitmentAgencies
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
             
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IGrade, GradeImplementation>();
            container.RegisterType<ICountry, CountryImplementation>();
            container.RegisterType<IApplicant, ApplicantImplementation>();
            container.RegisterType<IPasswordGenerator, PasswordGenerator>();
            container.RegisterType<IPasswordSettings, PasswordGeneratorSettings>();
            container.RegisterType<IEducation, EducationImplementation>();
            container.RegisterType<IUserPhoto, UserPhotoImplementation>();
            container.RegisterType<ICompany, CompanyImplementation>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}