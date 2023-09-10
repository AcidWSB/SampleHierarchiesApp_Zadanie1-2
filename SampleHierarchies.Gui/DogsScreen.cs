using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;

namespace SampleHierarchies.Gui;

/// <summary>
/// Mammals main screen.
/// </summary>
public sealed class DogsScreen : Screen
{
    #region Properties And Ctor

    /// <summary>
    /// Data service.
    /// </summary>
    private IDataService _dataService;
    /// <summary>
    /// Settings service.
    /// </summary>
    private SettingsService _settingsService;

    public override string? screenDefinitionJson { get; set; }
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="dataService">Data service reference</param>
    public DogsScreen(IDataService dataService, SettingsService settingsService)
    {
        _dataService = dataService;
        _settingsService = settingsService;
        screenDefinitionJson = "DogsScreenDefinition.json";
    }

    #endregion Properties And Ctor

    #region Public Methods

    /// <inheritdoc/>
    public override void Show()
    {
        _settingsService.SetColor("DogsScreen");
        if (screenDefinitionJson != null)
        {
            var menuEntries = ScreenDefinitionService.ReadMenu(screenDefinitionJson);

            DogsScreenChoices selectedOption = DogsScreenChoices.List;

            int selectedIndex = (int)selectedOption;

            while (true)
            {
                Console.Clear();

                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 23);
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
                        selectedOption = (DogsScreenChoices)selectedIndex;

                        switch (selectedOption)
                        {
                            case DogsScreenChoices.List:
                                ListDogs();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 22);
                                Console.ReadKey();
                                break;
                            case DogsScreenChoices.Create:
                                AddDog();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 22);
                                Console.ReadKey();
                                break;
                            case DogsScreenChoices.Delete:
                                DeleteDog();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 22);
                                Console.ReadKey();
                                break;
                            case DogsScreenChoices.Modify:
                                EditDogMain();
                                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 22);
                                Console.ReadKey();
                                break;
                            case DogsScreenChoices.Exit:
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

    #region Private Methods

    /// <summary>
    /// List all dogs.
    /// </summary>
    private void ListDogs()
    {
        if (screenDefinitionJson != null)
        {


            if (_dataService?.Animals?.Mammals?.Dogs is not null &&
                _dataService.Animals.Mammals.Dogs.Count > 0)
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
                int i = 1;
                foreach (Dog dog in _dataService.Animals.Mammals.Dogs)
                {
                    Console.Write($"Dog number {i}, ");
                    dog.Display();
                    i++;
                }
            }
            else
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 10);
            }
        }
        else
        {
            throw new Exception("Bad reading text from file");
        }
    }

    /// <summary>
    /// Add a dog.
    /// </summary>
    private void AddDog()
    {
        if (screenDefinitionJson != null)
        {
            try
            {
                Dog dog = AddEditDog();
                _dataService?.Animals?.Mammals?.Dogs?.Add(dog);
                Console.WriteLine("Dog with name: {0} has been added to a list of dogs", dog.Name);
            }
            catch
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 11);
            }
        }
        else
        {
            throw new Exception("Bad reading text from file");
        }
    }

    /// <summary>
    /// Deletes a dog.
    /// </summary>
    private void DeleteDog()
    {
        if (screenDefinitionJson != null)
        {
            try
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 12);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                Dog? dog = (Dog?)(_dataService?.Animals?.Mammals?.Dogs
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (dog is not null)
                {
                    _dataService?.Animals?.Mammals?.Dogs?.Remove(dog);
                    Console.WriteLine("Dog with name: {0} has been deleted from a list of dogs", dog.Name);
                }
                else
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 13);
                }
            }
            catch
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 14);
            }
        }
        else
        {
            throw new Exception("Bad reading text from file");
        }
    }

    /// <summary>
    /// Edits an existing dog after choice made.
    /// </summary>
    private void EditDogMain()
    {
        if (screenDefinitionJson != null)
        {
            try
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 15);
                string? name = Console.ReadLine();
                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                Dog? dog = (Dog?)(_dataService?.Animals?.Mammals?.Dogs
                    ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                if (dog is not null)
                {
                    Dog dogEdited = AddEditDog();
                    dog.Copy(dogEdited);
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 16);
                    dog.Display();
                }
                else
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 17);
                }
            }
            catch
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 18);
            }
        }
        else
        {
            throw new Exception("Bad reading text from file");
        }
    }

    /// <summary>
    /// Adds/edit specific dog.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private Dog AddEditDog()
    {
        if (screenDefinitionJson != null)
        {
            ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 19);
            string? name = Console.ReadLine();
            ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 20);
            string? ageAsString = Console.ReadLine();
            ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 21);
            string? breed = Console.ReadLine();

            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (ageAsString is null)
            {
                throw new ArgumentNullException(nameof(ageAsString));
            }
            if (breed is null)
            {
                throw new ArgumentNullException(nameof(breed));
            }
            int age = Int32.Parse(ageAsString);
            Dog dog = new Dog(name, age, breed);

            return dog;

        }
        else
        {
            throw new Exception("Bad reading text from file");
        }
    }

        #endregion // Private Methods
    }
