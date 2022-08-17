using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Data;
using SportsStore.Data.Enums;
using SportsStore.Data.Services;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService service;

        public CategoryController(ICategoryService service)
            => this.service = service;

        public async Task<IActionResult> Index()
        {
            var categories = await service.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> FilterBySport(int sportId)
        {
            var categories = await service.GetAllAsync();

            var result = categories.Where(c => sportId == c.KindOfSportId).ToList();

            return result is null ? View("NotFound") : View("Index", result);
        }

        public async Task<IActionResult> FilterByGender(Gender gender)
        {
            var categories = await service.GetAllAsync();

            var result = categories.Where(c => gender == c.Gender).ToList();

            return result is null ? View("NotFound") : View("Index", result);
        }
    }
}
