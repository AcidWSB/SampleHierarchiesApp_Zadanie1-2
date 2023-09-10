using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleHierarchies.Data.Services;
using SampleHierarchies.Enums;
using SampleHierarchies.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SampleHierarchies.Services
{
    public static class ScreenDefinitionService
    {
        public static ScreenDefinition? screenDefinition { get; set; }
        public static ScreenDefinition Load(string jsonFileName)
        {
            ScreenDefinition? loadedDefinition;
            if (File.Exists(jsonFileName))
            {
                string json = File.ReadAllText(jsonFileName);
                loadedDefinition = JsonConvert.DeserializeObject<ScreenDefinition>(json);
            }
            else
            {
                throw new Exception("File doesn't exists");
            }

            if (loadedDefinition != null)
                return loadedDefinition;
            else
                throw new Exception("Loading was failed");
        }

        public static void DisplayLineFromFile(string jsonFileName, int lineNumber)
        {
            screenDefinition = Load(jsonFileName);
            if (screenDefinition == null || screenDefinition.LineEntries == null || lineNumber > screenDefinition.LineEntries.Count)
            {
                Console.WriteLine("Invalid line number or screen definition is not loaded.");
                return;
            }           
            Console.ForegroundColor = screenDefinition.LineEntries[lineNumber].ForeGroundColor;
            Console.BackgroundColor = screenDefinition.LineEntries[lineNumber].BackGroundColor;  
            Console.WriteLine(screenDefinition.LineEntries[lineNumber].Text);
            Console.ResetColor();
        }

        public static void Serialize(string jsonFileName)
        {
            string json = JsonConvert.SerializeObject(screenDefinition, Formatting.Indented);
            File.WriteAllText(jsonFileName, json);
        }

        public static List<ScreenLineEntry> ReadMenu(string jsonFileName)
        {
            var menuEntries = new List<ScreenLineEntry>();
            if (File.Exists(jsonFileName))
            {
                string json = File.ReadAllText(jsonFileName);
                var screenDefinition = JsonConvert.DeserializeObject<ScreenDefinition>(json);

                if (screenDefinition != null && screenDefinition.LineEntries != null)
                {
                    foreach (var entry in screenDefinition.LineEntries)
                    {
                        if(entry.Text != null)
                            if (IsMenuItem(entry.Text))
                            {
                            menuEntries.Add(entry);
                            }
                    }
                }
                else
                {
                    throw new Exception("Invalid screen definition format");
                }
            }
            else
            {
                throw new Exception("File doesn't exist");
            }

            return menuEntries;
        }

        public static bool IsMenuItem(string text)
        {
            return !string.IsNullOrEmpty(text) && char.IsDigit(text[0]) && text.Length >= 2 && text[1] == '.';
        }

        public static void DisplayMenu(List<ScreenLineEntry> menuEntries, int selectedOption)
        {
            for (int i = 0; i < menuEntries.Count; i++)
            {
                var entry = menuEntries[i];
                if (i == selectedOption)
                {
                    Console.ForegroundColor = entry.ForeGroundColor;
                    Console.BackgroundColor = entry.BackGroundColor;
                }
                else
                {
                    Console.ForegroundColor = entry.BackGroundColor;
                    Console.BackgroundColor = entry.ForeGroundColor;
                }
            
                Console.WriteLine(entry.Text);
            }
            Console.ResetColor();
        }
    }
}
