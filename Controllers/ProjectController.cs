﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MwProject.Core.Models;
using MwProject.Core.Models.Domains;
using MwProject.Core.Services;
using MwProject.Core.ViewModels;
using MwProject.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {

        /* Konwencja ( Views \ nazwa_kontrolera \ nazwa_akcji.cshtml ) */

        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;

        private readonly int _itemPerPage = 2;

        /* Korzystając z mechanizmu DI wstrzykujemy zależności */
        public ProjectController(IProjectService projectService, ICategoryService categoryService)
        {
            _projectService = projectService;
            _categoryService = categoryService;
        }

        #region Lista projektów, pojedynczy projekt
        public IActionResult Projects(int currentPage = 1, int categoryId = 0)
        {
            var userId = User.GetUserId();
            
            int numberOfRecords = _projectService.GetNumberOfRecords(new ProjectsFilter(), categoryId);
            
            var projects = _projectService.GetProjects(new ProjectsFilter(),
                new PagingInfo() { CurrentPage = currentPage, ItemsPerPage = _itemPerPage },
                categoryId,
                userId
                );

            var vm = new ProjectsViewModel()
            {
                ProjectsFilter = new ProjectsFilter(),
                Categories = _categoryService.GetCategories(),
                Projects = projects,
                PagingInfo = new PagingInfo() { CurrentPage = currentPage, ItemsPerPage = _itemPerPage, TotalItems = numberOfRecords }
            };

            return View(vm);
        }

        public IActionResult Project(int id)
        {
            var userId = User.GetUserId();
            var selectedProject = id == 0 ?
                _projectService.NewProject(userId) :
                _projectService.GetProject(id,userId);

            var vm = new ProjectViewModel()
            {
                Project = selectedProject,
                Categories = _categoryService.GetCategories(),
                Heading = ""
            };

            return View(vm);
        }

        #endregion

        #region zapis
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Project(Project project)
        {
            var userId = User.GetUserId();

            if (!ModelState.IsValid)
            {
                var vm = new ProjectViewModel()
                {
                    Project = project,
                    Categories = _categoryService.GetCategories(),
                    Heading = project.Id == 0 ?
                     "nowy projekt" :
                     "edycja projektu"
                };

                return View("Project", vm);
            }

            if (project.Id == 0)
                _projectService.AddProject(project);
            else
                _projectService.UpdateProject(project,userId);


            return RedirectToAction("Projects", "Project");
        }


        [HttpPost]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var userId = User.GetUserId();
                _projectService.DeleteProject(id,userId);
            }
            catch (Exception ex)
            {
                // logowanie do pliku
                return Json(new { success = false, message = ex.Message + " ooo" });
            }

            return Json(new { success = true });
        }

        #endregion

    }
}