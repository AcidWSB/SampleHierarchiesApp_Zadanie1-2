using SampleHierarchies.Interfaces.Data;
using SampleHierarchies.Interfaces.Data.Mammals;

namespace SampleHierarchies.Data.Mammals;

/// <summary>
/// Mammals collection.
/// </summary>
public class Mammals : IMammals
{
    #region IMammals Implementation

    /// <inheritdoc/>
    public List<IDog> Dogs { get; set; }
    public List<ICat> Cats { get; set; }
    public List<IDeer> Deers { get; set; }
    public List<IPanda> Pandas { get; set; }


    #endregion // IMammals Implementation

    #region Ctors

    /// <summary>
    /// Default ctor.
    /// </summary>
    public Mammals()
    {
        Dogs = new List<IDog>();
        Cats = new List<ICat>();
        Deers = new List<IDeer>();
        Pandas = new List<IPanda>();
    }

    #endregion // Ctors
}
