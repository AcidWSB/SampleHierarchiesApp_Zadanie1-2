using Newtonsoft.Json;
using SampleHierarchies.Data;
using SampleHierarchies.Interfaces.Data;
using SampleHierarchies.Interfaces.Services;

namespace SampleHierarchies.Services;

/// <summary>
/// Settings service.
/// </summary>
public class SettingsService : ISettingsService, ISettings
{
    #region 

    /// <inheritdoc/>
    public Dictionary<string, ConsoleColor> ScreenColors { get; set; }
    public string JsonPath { get; set; }
    public string Version { get; set; }

    public SettingsService()
    {
        ScreenColors = new Dictionary<string, ConsoleColor>();
        JsonPath = "ScreenColors.json";
        Version = "1.0.0";
    }
    public void SerializeSettingsFile()
    {
        try
        {
            if (File.Exists(JsonPath))
            {
                string existingJson = File.ReadAllText(JsonPath);

                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    var existingMenuColors = JsonConvert.DeserializeObject<Dictionary<string, string>>(existingJson);

                    if (existingMenuColors != null)
                    {
                        foreach (var keyValuePair in ScreenColors)
                        {
                            existingMenuColors[keyValuePair.Key] = keyValuePair.Value.ToString();
                        }
                        string updatedJson = JsonConvert.SerializeObject(existingMenuColors, Formatting.Indented);

                      
                        File.WriteAllText(JsonPath, updatedJson);
                        Console.WriteLine("Settings successfully updated in 'ScreenColors.json'.");
                    }
                }
            }
            else
            {
                var serializedMenuColors = ScreenColors.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
                string json = JsonConvert.SerializeObject(serializedMenuColors, Formatting.Indented);

          
                File.WriteAllText(JsonPath, json);
                Console.WriteLine("Settings successfully serialized to 'ScreenColors.json'.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred during serialization: {ex.Message}");
        }
    }

    /// <summary>
    /// Deserialize menu color settings from a JSON file.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    private Dictionary<string, ConsoleColor> DeserializeSettingsFile()
    {
        try
        {
            string json = File.ReadAllText(JsonPath);
            var deserializedMenuColors = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (deserializedMenuColors != null)
            {
                var menuColors = deserializedMenuColors.ToDictionary(kv => kv.Key, kv => (ConsoleColor)Enum.Parse(typeof(ConsoleColor), kv.Value));
                return menuColors;
            }
            else
            {
                throw new Exception("JSON deserialization resulted in null.");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File {JsonPath} not found.");
            throw new FileNotFoundException($"File {JsonPath} not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred during deserialization: {ex.Message}");
            throw new Exception($"Error occurred during deserialization: {ex.Message}", ex);
        }
    }
    /// <summary>
    /// Method, that SetColor for Screen which user want
    /// </summary>
    /// <param name="screenName"></param>
    public void SetColor(string screenName)
    {
        Dictionary<string, ConsoleColor> colorsOfMenu = DeserializeSettingsFile();
        if (colorsOfMenu.ContainsKey(screenName))
        {
            ConsoleColor color = colorsOfMenu[screenName];
            Console.ForegroundColor = color;
        }
        else
        {
            Console.WriteLine($"Color for  '{screenName}' menu wasn't found.");
        }
    }
    public void EditSettingsFile()
    {
        ScreenColors = DeserializeSettingsFile();
        Console.WriteLine("Enter the name of the screen you want to change the color for: (MainScreen, AnimalScreen etc.)");
        string? screenName = Console.ReadLine();

        if (screenName != null && ScreenColors.ContainsKey(screenName))
        {
            // Display the current color for the selected screen
            Console.WriteLine($"Current color for screen '{screenName}': {ScreenColors[screenName]}");
            Console.WriteLine("Enter a new color (Red, Green, Yellow, etc.):");
            string? newColorStr = Console.ReadLine();

            // Attempt to parse the entered color into a ConsoleColor enum
            if (Enum.TryParse(typeof(ConsoleColor), newColorStr, out object? newColorObj) && newColorObj is ConsoleColor newColor)
            {
                // Set the new color for the screen
                ScreenColors[screenName] = newColor;
                Console.WriteLine($"Color for screen '{screenName}' successfully changed to {newColor}.");

                // Serialize the updated settings back to the file
                SerializeSettingsFile();
            }
            else
            {
                Console.WriteLine("Error: Invalid color entered.");
            }
        }
        else
        {
            Console.WriteLine($"Error: Screen with the name '{screenName}' not found in the settings file.");
        }
    }
    #endregion 
}