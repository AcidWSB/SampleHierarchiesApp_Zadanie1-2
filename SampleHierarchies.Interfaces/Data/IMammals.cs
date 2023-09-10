using SampleHierarchies.Interfaces.Data.Mammals;

namespace SampleHierarchies.Interfaces.Data;

/// <summary>
/// Mammals collection.
/// </summary>
public interface IMammals
{
    #region Interface Members

    /// <summary>
    /// Animal collections.
    /// </summary>
    List<IDog> Dogs { get; set; }
    List<ICat> Cats { get; set; }
    List<IDeer> Deers { get; set; }
    List<IPanda> Pandas { get; set; }

    #endregion // Interface Members
}
