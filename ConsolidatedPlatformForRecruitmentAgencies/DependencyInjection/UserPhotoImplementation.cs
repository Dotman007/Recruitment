using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConsolidatedPlatformForRecruitmentAgencies.Models;

namespace ConsolidatedPlatformForRecruitmentAgencies.DependencyInjection
{
    public class UserPhotoImplementation : IUserPhoto
    {
        public void UploadPhoto(HttpPostedFileBase photoFile, UserPhoto userPhoto)
        {
            if (photoFile != null)
            {
                string pic = System.IO.Path.GetFileName(photoFile.FileName);
                string path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/UserPicture"), pic);
                photoFile.SaveAs(path);
                userPhoto.UserPhotoImage = photoFile.FileName;
            }
        }
    }
}