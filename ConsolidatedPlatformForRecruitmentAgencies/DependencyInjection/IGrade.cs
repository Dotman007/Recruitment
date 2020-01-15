using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public interface IGrade
    {
        void AddGrade(Grade grade);
        List<Grade> GetAllGrades();
        void DeleteConfirm(int gradeId);
        Grade FindGradeById(int? id);
        void EditGrade(Grade grade);
        Grade EditGradeById(int id);
    }
}
