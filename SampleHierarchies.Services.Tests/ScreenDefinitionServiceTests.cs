using SampleHierarchies.Data.Services;
using SampleHierarchies.Services;

[TestClass]
public class ScreenDefinitionServiceTests
{
    private const string TestJsonFileName = "test.json";

    [TestInitialize]
    public void Initialize()
    {
        var screenDefinition = new ScreenDefinition
        {
            LineEntries = new List<ScreenLineEntry>
            {
                new ScreenLineEntry
                {
                    ForeGroundColor = ConsoleColor.Red,
                    BackGroundColor = ConsoleColor.White,
                    Text = "Hello"
                }
            }
        };

        string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(screenDefinition);
        File.WriteAllText(TestJsonFileName, jsonContent);
    }

    [TestMethod]
    public void Load_ExistingFile_ReturnsScreenDefinition()
    {
        // Act
        var screenDefinition = ScreenDefinitionService.Load(TestJsonFileName);

        // Assert
        Assert.IsNotNull(screenDefinition);
        Assert.IsNotNull(screenDefinition.LineEntries);
        Assert.AreEqual(1, screenDefinition.LineEntries.Count);

        var lineEntry = screenDefinition.LineEntries[0];
        Assert.AreEqual(ConsoleColor.Red, lineEntry.ForeGroundColor);
        Assert.AreEqual(ConsoleColor.White, lineEntry.BackGroundColor);
        Assert.AreEqual("Hello", lineEntry.Text);
    }

    [TestMethod]
    public void Load_NonExistingFile_ThrowsException()
    {
        // Arrange
        string nonExistentFileName = "nonexistent.json";

        // Act & Assert
        Assert.ThrowsException<Exception>(() => ScreenDefinitionService.Load(nonExistentFileName));
    }

    [TestMethod]
    public void DisplayLineFromFile_ValidLineNumber_DisplayText()
    {
        // Arrange
        int lineNumber = 0;

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            ScreenDefinitionService.DisplayLineFromFile(TestJsonFileName, lineNumber);

            var output = sw.ToString().Trim();

            // Assert
            Assert.AreEqual("Hello", output);
        }
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (File.Exists(TestJsonFileName))
        {
            File.Delete(TestJsonFileName);
        }
    }
}