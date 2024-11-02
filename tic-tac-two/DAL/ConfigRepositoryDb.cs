using GameBrain;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context;

    public ConfigRepositoryDb(AppDbContext context)
    {
        _context = context;
    }

    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();
        
        // Tagastab kõik konfiguratsioonide nimed, mis on andmebaasis
        return _context.GameConfigurations
            .OrderBy(config => config.Name)
            .Select(config => config.Name)
            .ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        var config = _context.GameConfigurations.FirstOrDefault(c => c.Name == name);

        if (config == null)
        {
            throw new KeyNotFoundException($"Configuration with name '{name}' not found.");
        }

        return config;
    }

    private void CheckAndCreateInitialConfig()
    {
        // Kontrollib, kas andmebaasis on juba konfiguratsioone
        if (!_context.GameConfigurations.Any())
        {
            // Kui konfiguratsioone pole, loo need kõvakodeeritud väärtuste põhjal
            var hardCodedRepo = new ConcreteConfigRepositoryHardCoded();
            var optionNames = hardCodedRepo.GetConfigurationNames();
            
            foreach (var optionName in optionNames)
            {
                var gameOption = hardCodedRepo.GetConfigurationByName(optionName);
                
                // Lisa kõvakodeeritud konfiguratsioonid andmebaasi
                _context.GameConfigurations.Add(gameOption);
            }
            
            _context.SaveChanges(); // Salvestab muudatused andmebaasi
        }
    }
}