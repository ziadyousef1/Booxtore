using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Booxtore.Application.Interfaces.Services;
using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Presentation.ViewModels;

namespace Booxtore.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IBookService bookService,
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository)
        {
            _logger = logger;
            _bookService = bookService;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedBooks = (await _bookService.GetFeaturedBooksAsync(6)).ToList(),
                BestSellingBooks = (await _bookService.GetPopularBooksAsync(8)).ToList(),
                LatestBooks = (await _bookService.GetLatestBooksAsync(6)).ToList(),
                Categories = (await _categoryRepository.GetAllAsync()).ToList(),
                TotalBooksCount = await _bookService.GetTotalBooksCountAsync(),
                TotalCategoriesCount = (await _categoryRepository.GetAllAsync()).Count(),
                TotalAuthorsCount = (await _authorRepository.GetAllAsync()).Count()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
