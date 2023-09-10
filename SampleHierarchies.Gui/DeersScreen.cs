using SampleHierarchies.Data.Mammals;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using SampleHierarchies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Gui
{
    public class DeersScreen : Screen
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
        public DeersScreen(IDataService dataService, SettingsService settingsService)
        {
            _dataService = dataService;
            _settingsService = settingsService;
            screenDefinitionJson = "DeersScreenDefinition.json";
        }

        #endregion Properties And Ctor

        #region Public Methods
        public override void Show()
        {
            _settingsService.SetColor("DeersScreen");
            if (screenDefinitionJson != null)
            {
                var menuEntries = ScreenDefinitionService.ReadMenu(screenDefinitionJson);

                DeersScreenChoices selectedOption = DeersScreenChoices.List;

                int selectedIndex = (int)selectedOption;

                while (true)
                {
                    Console.Clear();

                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 27);
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
                            selectedOption = (DeersScreenChoices)selectedIndex;

                            switch (selectedOption)
                            {
                                case DeersScreenChoices.List:
                                    ListDeers();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 26);
                                    Console.ReadKey();
                                    break;
                                case DeersScreenChoices.Create:
                                    AddDeer();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 26);
                                    Console.ReadKey();
                                    break;
                                case DeersScreenChoices.Delete:
                                    DeleteDeer();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 26);
                                    Console.ReadKey();
                                    break;
                                case DeersScreenChoices.Modify:
                                    EditDeerMain();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 26);
                                    Console.ReadKey();
                                    break;
                                case DeersScreenChoices.Exit:
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 7);
                                    Thread.Sleep(500);
                                    return;
                            }
                            break;
                    }
                }
            }
            else { throw new Exception("Bad reading menu from file"); }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// List all Deers.
        /// </summary>
        private void ListDeers()
        {
            if (screenDefinitionJson != null)
            {
                Console.WriteLine();
                if (_dataService?.Animals?.Mammals?.Deers is not null &&
                    _dataService.Animals.Mammals.Deers.Count > 0)
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
                    int i = 1;
                    foreach (Deer deer in _dataService.Animals.Mammals.Deers)
                    {
                        Console.Write($"Deer number {i}, ");
                        deer.Display();
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
        /// Add a deer.
        /// </summary>
        private void AddDeer()
        {
            if (screenDefinitionJson != null)
            {
                try
                {
                    Deer deer = AddEditDeer();
                    _dataService?.Animals?.Mammals?.Deers?.Add(deer);
                    Console.WriteLine("Deer with name: {0} has been added to a list of deers", deer.Name);
                }
                catch
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 11);
                }
            }else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Deletes a deer.
        /// </summary>
        private void DeleteDeer()
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
                    Deer? deer = (Deer?)(_dataService?.Animals?.Mammals?.Deers
                        ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                    if (deer is not null)
                    {
                        _dataService?.Animals?.Mammals?.Deers?.Remove(deer);
                        Console.WriteLine("Deer with name: {0} has been deleted from a list of deers", deer.Name);
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
        /// Edits an existing deer after choice made.
        /// </summary>
        private void EditDeerMain()
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
                    Deer? deer = (Deer?)(_dataService?.Animals?.Mammals?.Deers
                        ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                    if (deer is not null)
                    {
                        Deer deerEdited = AddEditDeer();
                        deer.Copy(deerEdited);
                        ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 16);
                        deer.Display();
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
            else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Adds/edit specific deer.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private Deer AddEditDeer()
        {
            if (screenDefinitionJson != null)
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 19);
                string? name = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 20);
                string? ageAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 21);
                string? antlerCountAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 22);
                string? lengthOfAntlersAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 23);
                string? speedAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 24);
                string? coatColor = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                string? habitat = Console.ReadLine();

                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                if (ageAsString is null)
                {
                    throw new ArgumentNullException(nameof(ageAsString));
                }
                if (antlerCountAsString is null)
                {
                    throw new ArgumentNullException(nameof(antlerCountAsString));
                }
                if (lengthOfAntlersAsString is null)
                {
                    throw new ArgumentNullException(nameof(lengthOfAntlersAsString));
                }
                if (speedAsString is null)
                {
                    throw new ArgumentNullException(nameof(speedAsString));
                }
                if (coatColor is null)
                {
                    throw new ArgumentNullException(nameof(coatColor));
                }
                if (habitat is null)
                {
                    throw new ArgumentNullException(nameof(habitat));
                }

                int age = int.Parse(ageAsString);
                int antlerCount = int.Parse(antlerCountAsString);
                double lengthOfAntlers = double.Parse(lengthOfAntlersAsString);
                double speed = double.Parse(speedAsString);
                Deer deer = new Deer(name, age, antlerCount, lengthOfAntlers, speed, coatColor, habitat);

                return deer;
            }else { throw new Exception("Bad reading text from file"); }
        }
        #endregion
    }
}
