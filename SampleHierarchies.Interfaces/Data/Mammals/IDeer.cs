using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Interfaces.Data.Mammals
{
    public interface IDeer : IMammal
    {
        int AntlerCount { get; set; }
        double LengthOfAntlers { get; set; }
        double Speed {get;set; }
        string CoatColor { get; set; }
        string Habitat { get; set; }
    }
}
