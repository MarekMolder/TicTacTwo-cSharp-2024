using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.WatchSomeoneGame;

public class WatchSomeoneGame(IGameRepository gameRepository) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string UserName { get; set; } = default!;
    
    public SelectList GameSelectList { get; set; } = default!;
    
    [BindProperty]
    public string GameName { get; set; } = default!;
    
    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(UserName)) return RedirectToPage("./Index", new { error = "No username provided." });
        
        ViewData["UserName"] = UserName;

        var selectedListData = gameRepository.GetSavedGameNames()
            .Select(name => new {id = name, value = name})
            .ToList();
        GameSelectList = new SelectList(selectedListData, "id", "value");
        
        return Page();
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(GameName))
        {
            ModelState.AddModelError(string.Empty, "Please select a game.");
            return Page();
        }
        else
        {
            return RedirectToPage("/PlayGame/Index", new { gameName = GameName, userName = UserName });
        }
    }
}