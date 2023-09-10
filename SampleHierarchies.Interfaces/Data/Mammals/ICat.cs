using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Interfaces.Data.Mammals
{
    public interface ICat : IMammal
    {
        #region Interface Members
        double TailLength { get; set; }
        string Personality { get; set; }
        string Hobbies { get; set; }
        string FavoriteSpot { get; set; }
        #endregion // Interface Members
    }
}
