using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ConsolidatedPlatformForRecruitmentAgencies.DAL;
using ConsolidatedPlatformForRecruitmentAgencies.Models;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public class GradeImplementation : IGrade
    {
        private readonly RecruitmentContext _recruitmentContext;
        public GradeImplementation(RecruitmentContext recruitmentContext)
        {
            _recruitmentContext = recruitmentContext;
        }

        #region Add Grade
        public void AddGrade(Grade grade)
        {
            if (grade != null)
            {
                _recruitmentContext.Grades.Add(grade);
                _recruitmentContext.SaveChanges();
            }
        }
        #endregion

        #region Get All Grades
        public List<Grade> GetAllGrades()
        {
            var grades =
            _recruitmentContext.Grades.ToList();
            return grades;
        }
        #endregion

        #region Delete Grade
        public void DeleteConfirm(int id)
        {
            var grade = _recruitmentContext.Grades.Find(id);
            if (grade != null)
            {
                _recruitmentContext.Grades.Remove(grade);
                _recruitmentContext.SaveChanges();
            }
        }
        #endregion


        #region Delete Grade
        public Grade FindGradeById(int? id)
        {
            var grade = _recruitmentContext.Grades.Find(id);
            return grade;
        }
        #endregion



        #region Edit Grade
        public Grade EditGradeById(int id)
        {
            var mygrade = _recruitmentContext.Grades.Where(g => g.GradeId == id).SingleOrDefault();
            return mygrade;
        }

        #endregion

        #region Edit Grade
        public void EditGrade(Grade grade)
        {
            _recruitmentContext.Entry(grade).State = EntityState.Modified;
            _recruitmentContext.SaveChanges();
        }
        #endregion


       
    }
}