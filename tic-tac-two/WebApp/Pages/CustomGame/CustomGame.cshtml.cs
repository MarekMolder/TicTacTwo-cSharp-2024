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
        
        [BindProperty, Required, Range(2, 20, ErrorMessage = "Board width must be between 2 and 20.")]
        public int BoardWidth { get; set; }
        
        [BindProperty, Required, Range(2, 20, ErrorMessage = "Board height must be between 2 and 20.")]
        public int BoardHeight { get; set; }

        [BindProperty, Required, Range(2, 100, ErrorMessage = "Piece number must be between 2 and 100.")]
        public int PieceNumber { get; set; }

        [BindProperty, Required, Range(2, 10, ErrorMessage = "Win condition must be between 2 and 10.")]
        public int WinCondition { get; set; }

        [BindProperty, Range(0, int.MaxValue, ErrorMessage = "Piece move after N move must be a positive number.")]
        public int PieceMove { get; set; }

        [BindProperty] public bool Grid { get; set; }
        
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
        
        [BindProperty(SupportsGet = true)] public string PlayerXorO{ get; set; } = default!;
        [BindProperty(SupportsGet = true)] public string NumberOfAIs{ get; set; } = default!;
        public List<string> Errors { get; set; } = new List<string>();
        

         public void OnGet()
        {
            BoardWidth = 3;
            BoardHeight = 3;
            PieceNumber = 5;
            WinCondition = 3;
            PieceMove = 100;
            Grid = false;
            
            GridWidth = 0;
            GridHeight = 0;
            GridMove = 0;
            GridX = 0;
            GridY = 0;
        }

        public IActionResult OnPost()
        {
            
            var errors = new List<string>();

            if (PieceNumber < WinCondition)
            {
                errors.Add($"WinCondition can't be bigger than {PieceNumber}.");
            }

            if (Grid)
            {
                if (GridWidth < WinCondition || GridWidth > BoardWidth)
                {
                    errors.Add($"Grid width must be between 0 and {BoardWidth}.");
                }

                if (GridHeight < WinCondition || GridHeight > BoardHeight)
                {
                    errors.Add($"Grid height must be between 0 and {BoardHeight}.");
                }
                
                if (GridX < 0 || GridX + GridWidth > BoardWidth)
                {
                    errors.Add("Grid position X is out of bounds.");
                }

                if (GridY < 0 || GridY + GridHeight > BoardHeight)
                {
                    errors.Add("Grid position Y is out of bounds.");
                }
            }
            
            if (errors.Any())
            {
                Errors = errors;
                return Page();
            }
            
            var config = new Domain.GameConfiguration()
            {
                Name = "CustomGame_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
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
            
            return RedirectToPage("/PlayGame/Index", new { configName = config.Name, username = UserName, playerXorO = PlayerXorO, numberOfAIs = NumberOfAIs});
        }
    }
}
