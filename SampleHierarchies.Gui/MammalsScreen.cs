using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;

namespace SampleHierarchies.Gui;

/// <summary>
/// Mammals main screen.
/// </summary>
public sealed class MammalsScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// Animals screen.
    /// </summary>
    private DogsScreen _dogsScreen;
    private CatsScreen _catsScreen;
    private DeersScreen _deersScreen;
    private PandasScreen _pandasScreen;
    /// <summary>
    /// Settings service.
    /// </summary>
    private SettingsService _settingsService;

    public override string? screenDefinitionJson { get; set; }

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService">Data service reference</param>
    /// <param name="dogsScreen">Dogs screen</param>
    public MammalsScreen(DogsScreen dogsScreen, SettingsService settingsService, CatsScreen catsScreen, DeersScreen deersScreen, PandasScreen pandasScreen)
    {
        _settingsService = settingsService;
        _dogsScreen = dogsScreen;
        _catsScreen = catsScreen;
        _deersScreen = deersScreen;
        _pandasScreen = pandasScreen;
        screenDefinitionJson = "MammalScreenDefinition.json";
    }

    #endregion Properties And Ctor

    #region Public Methods

    /// <inheritdoc/>
    public override void Show()
    {
        _settingsService.SetColor("MammalsScreen");
        if (screenDefinitionJson != null)
        {
            var menuEntries = ScreenDefinitionService.ReadMenu(screenDefinitionJson);

            MammalsScreenChoices selectedOption = MammalsScreenChoices.Dogs;

            int selectedIndex = (int)selectedOption;

            while (true)
            {
                Console.Clear();

                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 10);
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
                        selectedOption = (MammalsScreenChoices)selectedIndex;

                        switch (selectedOption)
                        {
                            case MammalsScreenChoices.Dogs:
                                _dogsScreen.Show();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
                                Console.ReadKey();
                                break;
                            case MammalsScreenChoices.Cats:
                                _catsScreen.Show();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
                                Console.ReadKey();
                                break;
                            case MammalsScreenChoices.Deers:
                                _deersScreen.Show();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
                                Console.ReadKey();
                                break;
                            case MammalsScreenChoices.Pandas:
                                _pandasScreen.Show();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
                                Console.ReadKey();
                                break;
                            case MammalsScreenChoices.Exit:
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 7);
                                Thread.Sleep(500);
                                return;
                        }
                        break;
                }
            }
        }else { throw new Exception("Bad reading menu from file"); }
    }

    #endregion // Public Methods
}
