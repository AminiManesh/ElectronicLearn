using ElectronicLearn.Core.DTOs;
using ElectronicLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services.Interfaces
{
    public interface ICourseService
    {
        #region Group
        List<CourseGroup> GetAllCourseGroups();
        List<SelectListItem> GetAllParentGroupsForManageCourse();
        List<CourseGroup> GetAllParentGroups();
        List<SelectListItem> GetAllSubGroupsForManageCourse(int parentId);
        List<SelectListItem> GetAllCourseStatuses();
        List<SelectListItem> GetAllCourseLevels();
        #endregion


        #region Course
        bool IsUserHasCourse(int userId, int courseId);
        AdminCourseListViewModel GetAllCoursesForAdmin(int pageId = 1, int groupId = 0, int subGroupId = 0, string courseName = "");
        int AddCourse(Course course, IFormFile demoVideo, IFormFile courseImage);
        Course GetCourseById(int courseId);
        Course GetCourseDetails(int courseId);
        void UpdateCourse(Course course, IFormFile demoVideo, IFormFile courseImage);
        Tuple<List<CourseListItemViewModel>, int> GetCourses(int pageId = 1, string filter = "", string priceType = "all", string orderBy = "createDate", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null, int take = 0);
        List<CourseListItemViewModel> GetPopularCourses();
        #endregion


        #region Episode
        AdminEpisodeListViewModel GetCourseEpisodes(int courseId);
        bool IsFileExists(IFormFile file, string folderPath);
        AdminEpisodeViewModel GetEpisodeForEdit(int episodeId);
        CourseEpisode GetEpisodeById(int episodeId);
        void AddEpisode(AdminEpisodeViewModel episode);
        void UpdateEpisode(AdminEpisodeViewModel episode);
        #endregion

        #region Comment
        void AddComment(CourseComment comment);
        Tuple<List<CourseComment>, int> GetCourseComments(int courseId, int pageId = 1);
        #endregion
    }
}
