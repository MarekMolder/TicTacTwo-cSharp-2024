using System.ComponentModel.DataAnnotations;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CustomGame
{
    public class CustomGame : PageModel
    {
        private readonly IConfigRepository _configRepository;

        public CustomGame(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }
        
        [BindProperty, Required, Range(2, 20)]
        public int BoardWidth { get; set; }
        
        [BindProperty, Required, Range(2, 20)]
        public int BoardHeight { get; set; }

        [BindProperty, Required, Range(2, 100)]
        public int PieceNumber { get; set; }

        [BindProperty, Required, Range(2, 10)]
        public int WinCondition { get; set; }

        [BindProperty]
        public int PieceMove { get; set; }

        [BindProperty]
        public bool Grid { get; set; }

        // Grid-related properties
        [BindProperty]
        public int GridWidth { get; set; }
        
        [BindProperty]
        public int GridHeight { get; set; }

        [BindProperty]
        public int GridMove { get; set; }

        [BindProperty]
        public int GridX { get; set; }

        [BindProperty]
        public int GridY { get; set; }
        
        [BindProperty(SupportsGet = true)] public string UserName { get; set; } = default!;

        public void OnGet()
        {
            // Default values for board and piece settings
            BoardWidth = 3;
            BoardHeight = 3;
            PieceNumber = 5;
            WinCondition = 3;
            PieceMove = 100;
            Grid = false; // default, no grid

            // Default grid values when Grid is not checked
            GridWidth = 0;
            GridHeight = 0;
            GridMove = 0;
            GridX = 0;
            GridY = 0;
        }

        public IActionResult OnPost()
        {
            var config = new Domain.GameConfiguration()
            {
                Name = "CustomGame_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                BoardSizeWidth = BoardWidth,
                BoardSizeHeight = BoardHeight,
                PiecesNumber = PieceNumber,
                WinCondition = WinCondition,
                MovePieceAfterNMove = PieceMove,
                UsesGrid = Grid,
                GridSizeWidth = Grid ? GridWidth : 0,
                GridSizeHeight = Grid ? GridHeight : 0,
                MoveGridAfterNMove = Grid ? GridMove : 10000,
                GridPositionX = Grid ? GridX : 0,
                GridPositionY = Grid ? GridY : 0
            };
            
            _configRepository.SaveConfiguration(config);
               
                
            return RedirectToPage("/PlayGame/Index", new { configName = config.Name, username = UserName });
        }
    }
}
