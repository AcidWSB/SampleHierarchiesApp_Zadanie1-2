using SampleHierarchies.Data.Services;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;

namespace SampleHierarchies.Gui;

/// <summary>
/// Application main screen.
/// </summary>
public sealed class MainScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// Data service.
    /// </summary>
    private IDataService _dataService;

    /// <summary>
    /// Animals screen.
    /// </summary>
    private AnimalsScreen _animalsScreen;

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
    public MainScreen(
        IDataService dataService,
        SettingsService settingsService,
        AnimalsScreen animalsScreen)
    {
        _dataService = dataService;
        _settingsService = settingsService;
        _animalsScreen = animalsScreen;
        screenDefinitionJson = "MainScreenDefinition.json";
    }

    #endregion Properties And Ctor

    #region Public Methods

    /// <inheritdoc/>
    public override void Show()
    {
        _settingsService.SetColor("MainScreen");
        if (screenDefinitionJson != null)
        {
            var menuEntries = ScreenDefinitionService.ReadMenu(screenDefinitionJson);

            MainScreenChoices selectedOption = MainScreenChoices.Exit;

            int selectedIndex = (int)selectedOption;



            while (true)
            {
                Console.Clear();

                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 7);
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
                        selectedOption = (MainScreenChoices)selectedIndex;

                        switch (selectedOption)
                        {
                            case MainScreenChoices.Animals:
                                Console.Clear();
                                _animalsScreen.Show();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 6);
                                Console.ReadKey();
                                break;
                            case MainScreenChoices.Settings:
                                _settingsService.EditSettingsFile();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 6);
                                Console.ReadKey();
                                break;
                            case MainScreenChoices.Exit:
                                Console.Clear();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 6);
                                return;
                        }
                        break;
                }
            }
        }else { throw new Exception("Bad reading menu from file"); }
    }
    #endregion
}