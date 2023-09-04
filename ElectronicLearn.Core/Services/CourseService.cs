using ElectronicLearn.Core.DTOs;
using ElectronicLearn.Core.Security;
using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.Core.Tools;
using ElectronicLearn.DataLayer.Context;
using ElectronicLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly ElectronicLearnContext _context;

        public CourseService(ElectronicLearnContext context)
        {
            _context = context;
        }
        // Add course from admin panel
        public int AddCourse(Course course, IFormFile demoVideo, IFormFile courseImage)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "no-image.jpg";

            //Todo Check image
            if (courseImage != null && courseImage.IsImage())
            {
                course.CourseImageName = FileTools.SaveFile(courseImage, "wwwroot/Courses/Image");

                // Resize and save thumbnail
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/Image", course.CourseImageName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/Thumbnail", course.CourseImageName);
                FileTools.SaveThumbnailAsync(path, savePath);
            }

            //Todo upload demo
            if (demoVideo != null)
            {
                course.DemoFileName = FileTools.SaveFile(demoVideo, "wwwroot/Courses/Demoes");
            }

            _context.Add(course);
            _context.SaveChanges();

            return course.CourseId;
        }
        
        public void AddEpisode(AdminEpisodeViewModel episode)
        {
            var CourseEpisode = new CourseEpisode();
            CourseEpisode.CourseId = episode.CourseId;
            CourseEpisode.CourseEpisodeTitle = episode.CourseEpisodeTitle;
            CourseEpisode.IsFree = episode.IsFree;
            CourseEpisode.EpisodeTime = episode.EpisodeTime;

            if (episode.EpisodeFile != null)
            {
                var courseEpisodesPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Courses/Episodes/{episode.CourseId}/");
                if (!Directory.Exists(courseEpisodesPath))
                {
                    Directory.CreateDirectory(courseEpisodesPath);
                }
                CourseEpisode.EpisodeFileName = FileTools.SaveFile(episode.EpisodeFile, $"wwwroot/Courses/Episodes/{episode.CourseId}/");
            }

            _context.Add(CourseEpisode);
            _context.SaveChanges();
        }

        public List<CourseGroup> GetAllCourseGroups()
        {
            return _context.CourseGroups.ToList();
        }

        public List<SelectListItem> GetAllCourseLevels()
        {
            return _context.CourseLevels
                .Select(cl => new SelectListItem()
                {
                    Text = cl.CourseLevelTitle,
                    Value = cl.CourseLevelId.ToString()
                }).ToList();
        }

        public AdminCourseListViewModel GetAllCoursesForAdmin(int pageId = 1, int groupId = 0, int subGroupId = 0, string courseName = "")
        {
            IQueryable<Course> courses = _context.Courses;

            if (!string.IsNullOrEmpty(courseName))
            {
                courses = courses.Where(c => c.CourseTitle.Contains(courseName));
            }

            if (groupId != 0)
            {
                courses = courses.Where(c => c.GroupId == groupId);
            }

            if (subGroupId != 0)
            {
                courses = courses.Where(c => c.SubGroupId == subGroupId);
            }


            // Show items in each page
            int take = 5;
            int skip = (pageId - 1) * take;

            var result = new AdminCourseListViewModel();

            result.CurrentPage = pageId;

            var count = courses.Count();
            if (count % take == 0)
            {
                result.PagesCount = count / take;
            }
            else if (count % take > 0)
            {
                result.PagesCount = (count / take) + 1;
            }

            result.Courses = courses
                .OrderBy(c => c.CreateDate)
                .Skip(skip)
                .Take(take)
                .Include(c => c.Group)
                .Select(c => new AdminCourseViewModel()
                {
                    CourseTitle = c.CourseTitle,
                    CourseId = c.CourseId,
                    CourseImageName = c.CourseImageName,
                    EpisodeCount = _context.CourseEpisodes.Count(ce => ce.CourseId == c.CourseId),
                    GroupName = c.Group.CourseGroupTitle,
                    CoursePrice = c.CoursePrice
                }).ToList();

            return result;
        }

        public List<SelectListItem> GetAllCourseStatuses()
        {
            return _context.CourseStatuses
                .Select(cs => new SelectListItem()
                {
                    Text = cs.CourseStatusTitle,
                    Value = cs.CourseStatusId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetAllParentGroupsForManageCourse()
        {
            return _context.CourseGroups
                .Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.CourseGroupTitle,
                    Value = g.CourseGroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetAllSubGroupsForManageCourse(int parentId)
        {
            return _context.CourseGroups
                .Where(g => g.ParentId == parentId)
                .Select(g => new SelectListItem()
                {
                    Text = g.CourseGroupTitle,
                    Value = g.CourseGroupId.ToString()
                }).ToList();
        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public AdminEpisodeListViewModel GetCourseEpisodes(int courseId)
        {
            var result = new AdminEpisodeListViewModel();
            result.EpisodeList = _context.CourseEpisodes
                .Where(e => e.CourseId == courseId)
                .Select(e => new AdminEpisodeViewModel()
                {
                    CourseEpisodeId = e.CourseEpisodeId,
                    CourseId = e.CourseId,
                    CourseEpisodeTitle = e.CourseEpisodeTitle,
                    EpisodeTime = e.EpisodeTime,
                    IsFree = e.IsFree
                }).ToList();
            result.CourseId = courseId;
            result.CourseName = _context.Courses.Find(courseId).CourseTitle;

            return result;
        }

        // Update and edit course from admin panel
        public void UpdateCourse(Course course, IFormFile demoVideo, IFormFile courseImage)
        {
            course.UpdateDate = DateTime.Now;

            //Todo Check image
            if (courseImage != null && courseImage.IsImage())
            {
                bool deletePrevious = (course.CourseImageName != "no-image.jpg") ? true : false;

                if (deletePrevious)
                {
                    FileTools.DeletePreviousFile("wwwroot/Courses/Thumbnail", course.CourseImageName);
                }

                course.CourseImageName = FileTools.SaveFile(courseImage, "wwwroot/Courses/Image", deletePrevious, course.CourseImageName);

                // Resize and save thumbnail

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/Image", course.CourseImageName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Courses/Thumbnail", course.CourseImageName);
                FileTools.SaveThumbnailAsync(path, savePath);
            }

            //Todo upload demo
            if (demoVideo != null)
            {
                if (course.DemoFileName != null)
                {
                    FileTools.DeletePreviousFile("wwwroot/Courses/Demoes", course.DemoFileName);
                }
                course.DemoFileName = FileTools.SaveFile(demoVideo, "wwwroot/Courses/Demoes");
            }

            _context.Courses.Update(course);
            _context.SaveChanges();
        }
    }
}
