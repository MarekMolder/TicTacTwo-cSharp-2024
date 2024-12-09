using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CustomGame
{
    public class CustomGame : PageModel
    {
        // These properties will bind to the form inputs in the Razor Page
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
        [BindProperty, Range(2, 20)]
        public int GridWidth { get; set; }
        
        [BindProperty, Range(2, 20)]
        public int GridHeight { get; set; }

        [BindProperty]
        public int GridMove { get; set; }

        [BindProperty]
        public int GridX { get; set; }

        [BindProperty]
        public int GridY { get; set; }

        public void OnGet()
        {
            // Initialize any default values here, if necessary
            // Example: Set default values for board dimensions and piece count
            BoardWidth = 3;   // Default value
            BoardHeight = 3;  // Default value
            PieceNumber = 5; // Default number of pieces (e.g., chess pieces)
            WinCondition = 3; // Default win condition (e.g., 4 in a row)
            PieceMove = 100;    // Default piece movement
            Grid = false;     // Default value, no grid by default

            // Optionally, initialize grid-related properties if needed
            GridWidth = 0;
            GridHeight = 0;
            GridMove = 0;
            GridX = 0;
            GridY = 0;
        }

        // This method will handle form submission via POST request
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                return RedirectToPage("/PlayGame/Index", new 
                {
                    boardWidth = BoardWidth,
                    boardHeight = BoardHeight,
                    pieceNumber = PieceNumber,
                    winCondition = WinCondition,
                    pieceMove = PieceMove,
                    grid = Grid,
                    gridWidth = Grid ? GridWidth : 0,
                    gridHeight = Grid ? GridHeight : 0,
                    gridMove = Grid ? GridMove : 10000,
                    gridX = Grid ? GridX : 0,
                    gridY = Grid ? GridY : 0
                });
                
            }

            // If the model is not valid, return the same page with validation messages
            return Page();
        }
    }
}
