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
    public class PandasScreen : Screen
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
        public PandasScreen(IDataService dataService, SettingsService settingsService)
        {
            _dataService = dataService;
            _settingsService = settingsService;
            screenDefinitionJson = "PandasScreenDefinition.json";
        }

        #endregion Properties And Ctor

        #region Public Methods
        public override void Show()
        {
            _settingsService.SetColor("PandasScreen");
            if (screenDefinitionJson != null)
            {
                var menuEntries = ScreenDefinitionService.ReadMenu(screenDefinitionJson);

                PandasScreenChoices selectedOption = PandasScreenChoices.List;

                int selectedIndex = (int)selectedOption;

                while (true)
                {
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
                            selectedOption = (PandasScreenChoices)selectedIndex;

                            switch (selectedOption)
                            {
                                case PandasScreenChoices.List:
                                    ListPandas();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                                    Console.ReadKey();
                                    break;
                                case PandasScreenChoices.Create:
                                    AddPanda();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                                    Console.ReadKey();
                                    break;
                                case PandasScreenChoices.Delete:
                                    DeletePanda();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                                    Console.ReadKey();
                                    break;
                                case PandasScreenChoices.Modify:
                                    EditPandaMain();
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                                    Console.ReadKey();
                                    break;
                                case PandasScreenChoices.Exit:
                                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 7);
                                    Thread.Sleep(500);
                                    return;
                            }
                            break;
                    }
                }
            }else { throw new Exception(); }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// List all Pandas.
        /// </summary>
        private void ListPandas()
        {
            if (screenDefinitionJson != null)
            {
                Console.WriteLine();
                if (_dataService?.Animals?.Mammals?.Pandas is not null &&
                    _dataService.Animals.Mammals.Pandas.Count > 0)
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 9);
                    int i = 1;
                    foreach (Panda panda in _dataService.Animals.Mammals.Pandas)
                    {
                        Console.Write($"Panda number {i}, ");
                        panda.Display();
                        i++;
                    }
                }
                else
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 10);
                }
            } else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Add a panda.
        /// </summary>
        private void AddPanda()
        {
            if (screenDefinitionJson != null)
            {
                try
                {
                    Panda panda = AddEditPanda();
                    _dataService?.Animals?.Mammals?.Pandas?.Add(panda);
                    Console.WriteLine("Panda with name: {0} has been added to a list of pandas", panda.Name);
                }
                catch
                {
                    ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 11);
                }
            } else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Deletes a panda.
        /// </summary>
        private void DeletePanda()
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
                    Panda? panda = (Panda?)(_dataService?.Animals?.Mammals?.Pandas
                        ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                    if (panda is not null)
                    {
                        _dataService?.Animals?.Mammals?.Pandas?.Remove(panda);
                        Console.WriteLine("Panda with name: {0} has been deleted from a list of pandas", panda.Name);
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
            } else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Edits an existing Panda after choice made.
        /// </summary>
        private void EditPandaMain()
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
                    Panda? panda = (Panda?)(_dataService?.Animals?.Mammals?.Pandas
                        ?.FirstOrDefault(d => d is not null && string.Equals(d.Name, name)));
                    if (panda is not null)
                    {
                        Panda pandaEdited = AddEditPanda();
                        panda.Copy(pandaEdited);
                        ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 16);
                        panda.Display();
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
            }else { throw new Exception("Bad reading text from file"); }
        }

        /// <summary>
        /// Adds/edit specific panda.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private Panda AddEditPanda()
        {
            if (screenDefinitionJson != null)
            {
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 19);
                string? name = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 20);
                string? ageAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 21);
                string? kindOf = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 22);
                string? spotCountAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 23);
                string? pawSizeAsString = Console.ReadLine();
                ScreenDefinitionService.DisplayLineFromFile(screenDefinitionJson, 25);
                string? socialBehavior = Console.ReadLine();

                if (name is null)
                {
                    throw new ArgumentNullException(nameof(name));
                }
                if (ageAsString is null)
                {
                    throw new ArgumentNullException(nameof(ageAsString));
                }
                if (kindOf is null)
                {
                    throw new ArgumentNullException(nameof(kindOf));
                }
                if (spotCountAsString is null)
                {
                    throw new ArgumentNullException(nameof(spotCountAsString));
                }
                if (pawSizeAsString is null)
                {
                    throw new ArgumentNullException(nameof(pawSizeAsString));
                }
                if (socialBehavior is null)
                {
                    throw new ArgumentNullException(nameof(socialBehavior));
                }

                int age = int.Parse(ageAsString);
                int spotCount = int.Parse(spotCountAsString);
                double pawSize = double.Parse(pawSizeAsString);
                Panda panda = new Panda(name, age, kindOf, spotCount, pawSize, socialBehavior);

                return panda;
            }
            else { throw new Exception("Bad reading text from file"); }
        }
        #endregion
    }
}
