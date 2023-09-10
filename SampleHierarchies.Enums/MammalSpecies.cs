using System.ComponentModel;
/// <summary>
/// Dummy enum.
/// </summary>
public enum MammalSpecies
{
    [Description("Simple description of a none")]
    None = 0,
    [Description("Simple description of a dog")]
    Dog = 1,
    [Description("Simple description of a cat")]
    Cat = 2,
    [Description("Simple description of a deer")]
    Deer = 3,
    [Description("Simple description of a panda")]
    Panda = 4,
}
