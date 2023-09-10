using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;

namespace SampleHierarchies.Gui;

/// <summary>
/// Animals main screen.
/// </summary>
public sealed class AnimalsScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// Data service.
    /// </summary>
    private IDataService _dataService;

    /// <summary>
    /// Animals screen.
    /// </summary>
    private MammalsScreen _mammalsScreen;
    /// <summary>
    /// Settings service.
    /// </summary>
    private SettingsService _settingsService;

    public override string? screenDefinitionJson { get; set; }

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService">Data service reference</param>
    /// <param name="animalsScreen">Animals screen</param>
    public AnimalsScreen(
        IDataService dataService,
        SettingsService settingsService,
        MammalsScreen mammalsScreen)
    {
        _dataService = dataService;
        _settingsService = settingsService;
        _mammalsScreen = mammalsScreen;
        screenDefinitionJson = "AnimalScreenDefinition.json";
    }

    #endregion Properties And Ctor

    #region Public Methods

    /// <inheritdoc/>
    public override void Show()
    {
        _settingsService.SetColor("AnimalScreen");
        if (screenDefinitionJson != null)
        {
            var menuEntries = ScreenDefinitionService.ReadMenu(screenDefinitionJson);

            AnimalsScreenChoices selectedOption = AnimalsScreenChoices.Exit;

            int selectedIndex = (int)selectedOption;

            while (true)
            {
                Console.Clear();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 15);
                ScreenDefinitionService.DisplayMenu(menuEntries, selectedIndex);

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(menuEntries.Count - 1, selectedIndex + 1);
                        break;
                    case ConsoleKey.Enter:
                        selectedOption = (AnimalsScreenChoices)selectedIndex;

                        switch (selectedOption)
                        {
                            case AnimalsScreenChoices.Mammals:
                                _mammalsScreen.Show();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 14);
                                Console.ReadKey();
                                break;
                            case AnimalsScreenChoices.Read:
                                ReadFromFile();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 14);
                                Console.ReadKey();
                                break;
                            case AnimalsScreenChoices.Save:
                                SaveToFile();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 14);
                                Console.ReadKey();
                                break;
                            case AnimalsScreenChoices.Exit:
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 6);
                                Thread.Sleep(500);
                                return;
                        }
                        break;
                }
            }
        }else { throw new Exception("Bad reading menu from file"); }
    }

    #endregion // Public Methods

    #region Private Methods

    /// <summary>
    /// Save to file.
    /// </summary>
    private void SaveToFile()
    {
        if (screenDefinitionJson != null)
        {
            try
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 8);
                var fileName = Console.ReadLine();
                if (fileName is null)
                {
                    throw new ArgumentNullException(nameof(fileName));
                }
                _dataService.Write(fileName);
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
            }
            catch
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 10);
            }
        }
    }

    /// <summary>
    /// Read data from file.
    /// </summary>
    private void ReadFromFile()
    {
        if (screenDefinitionJson != null)
        { 
            try
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 11);
                var fileName = Console.ReadLine();
                if (fileName is null)
                {
                    throw new ArgumentNullException(nameof(fileName));
                }
                _dataService.Write(fileName);
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 12);
            }
            catch
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 13);
            }
        }
    }

    #endregion // Private Methods
}
