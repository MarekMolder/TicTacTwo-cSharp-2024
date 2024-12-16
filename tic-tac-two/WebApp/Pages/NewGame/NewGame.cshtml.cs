using System.Runtime.Intrinsics.Arm;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.NewGame;

public class NewGame : PageModel
{
    private readonly IConfigRepository _configRepository;

    public NewGame(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }
    
    [BindProperty(SupportsGet = true)]
    public string UserName { get; set; } = default!;
    [BindProperty]
    public string PlayerXorO { get; set; } = default!;

    [BindProperty]
    public string NumberOfAIs { get; set; } = default!;
    
    public SelectList ConfigSelectList { get; set; } = default!;
    
    [BindProperty]
    public string ConfigName { get; set; } = default!;
    
    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(UserName)) return RedirectToPage("./Index", new { error = "No username provided." });
        
        ViewData["UserName"] = UserName;

        var selectedListData = _configRepository.GetConfigurationNames()
            .Select(name => new {id = name, value = name})
            .ToList();
        ConfigSelectList = new SelectList(selectedListData, "id", "value");
        
        return Page();
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(ConfigName))
        {
            ModelState.AddModelError(string.Empty, "Please select a configuration.");
            return Page();
        }
        
        if (ConfigName == "Custom")
        {
            return RedirectToPage("/CustomGame/CustomGame", new { userName = UserName, playerXor = PlayerXorO, numberOfAIs = NumberOfAIs });
        }
        else
        {
            var selectedConfig = _configRepository.GetConfigurationByName(ConfigName);
            
            return RedirectToPage("/PlayGame/Index", new { configName = selectedConfig.Name, userName = UserName, playerXorO = PlayerXorO, numberOfAIs = NumberOfAIs});
        }
    }
}