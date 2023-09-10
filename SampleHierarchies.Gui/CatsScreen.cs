using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Gui
{
    public class CatsScreen : Screen
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
        public CatsScreen(IDataService dataService, SettingsService settingsService)
        {
            _dataService = dataService;
            _settingsService = settingsService;
            screenDefinitionJson = "CatsScreenDefinition.json";
        }

        #endregion Properties And Ctor

        #region Public Methods
        public override void Show()
        {
            if (screenDefinitionJson != null)
            {
                var menuEntries = ScreenDefinitionService.ReadMenu(screenDefinitionJson);

                CatsScreenChoices selectedOption = CatsScreenChoices.List;

                int selectedIndex = (int)selectedOption;

                while (true)
                {
                    _settingsService.SetColor("CatsScreen");
                    Console.Clear();

                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 26);
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
                            selectedOption = (CatsScreenChoices)selectedIndex;

                            switch (selectedOption)
                            {
                                case CatsScreenChoices.List:
                                    ListCats();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                                    Console.ReadKey();
                                    break;
                                case CatsScreenChoices.Create:
                                    AddCat();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                                    Console.ReadKey();
                                    break;
                                case CatsScreenChoices.Delete:
                                    DeleteCat();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                                    Console.ReadKey();
                                    break;
                                case CatsScreenChoices.Modify:
                                    EditCatMain();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                                    Console.ReadKey();
                                    break;
                                case CatsScreenChoices.Exit:
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 7);
                                    Thread.Sleep(500);
                                    return;
                            }
                            break;
                    }
                }
            }else { throw new Exception("Bad reading menu from file"); }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// List all Cats.
        /// </summary>
        private void ListCats()
        {
            if (screenDefinitionJson != null)
            {
                Console.WriteLine();
                if (_dataService?.Animals?.Mammals?.Cats is not null &&
                    _dataService.Animals.Mammals.Cats.Count > 0)
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
                    int i = 1;
                    foreach (Cat cat in _dataService.Animals.Mammals.Cats)
                    {
                        Console.Write($"Cat number {i}, ");
                        cat.Display();
                        i++;
                    }
                }
                else
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 10);
                }
            }
            else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Add a cat.
        /// </summary>
        private void AddCat()
        {
            if (screenDefinitionJson != null)
            {
                try
                {
                    Cat cat = AddEditCat();
                    _dataService?.Animals?.Mammals?.Cats?.Add(cat);
                    Console.WriteLine("Cat with name: {0} has been added to a list of cats", cat.Name);
                }
                catch
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 11);
                }
            }else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Deletes a cat.
        /// </summary>
        private void DeleteCat()
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
                    Cat? cat = (Cat?)(_dataService?.Animals?.Mammals?.Cats
                        ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                    if (cat is not null)
                    {
                        _dataService?.Animals?.Mammals?.Cats?.Remove(cat);
                        Console.WriteLine("Cat with name: {0} has been deleted from a list of cats", cat.Name);
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
            else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Edits an existing cat after choice made.
        /// </summary>
        private void EditCatMain()
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
                    Cat? cat = (Cat?)(_dataService?.Animals?.Mammals?.Cats
                        ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                    if (cat is not null)
                    {
                        Cat catEdited = AddEditCat();
                        cat.Copy(catEdited);
                        ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 16);
                        cat.Display();
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
            else { throw new Exception("Bad reading text from file");}
        }

        /// <summary>
        /// Adds/edit specific cat.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private Cat AddEditCat()
        {
            if (screenDefinitionJson != null)
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 19);
                string? name = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 20);
                string? ageAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 21);
                string? tailLengthAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 22);
                string? personality = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 23);
                string? hobbies = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 24);
                string? favoriteSpot = Console.ReadLine();

                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                if (ageAsString is null)
                {
                    throw new ArgumentNullException(nameof(ageAsString));
                }
                if (tailLengthAsString is null)
                {
                    throw new ArgumentNullException(nameof(tailLengthAsString));
                }
                if (personality is null)
                {
                    throw new ArgumentNullException(nameof(personality));
                }
                if (hobbies is null)
                {
                    throw new ArgumentNullException(nameof(hobbies));
                }
                if (favoriteSpot is null)
                {
                    throw new ArgumentNullException(nameof(favoriteSpot));
                }
                int age = Int32.Parse(ageAsString);
                double tailLength = Double.Parse(tailLengthAsString);
                Cat cat = new Cat(name, age, tailLength, personality, hobbies, favoriteSpot);

                return cat;
            }
            else { throw new Exception("Bad reading text from file"); }
        }
        #endregion
    }
}
