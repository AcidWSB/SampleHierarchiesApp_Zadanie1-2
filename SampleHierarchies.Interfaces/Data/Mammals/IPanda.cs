using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Interfaces.Data.Mammals
{
    public interface IPanda : IMammal
    {
        string KindOf { get; set; }
        int SpotCount { get; set; }
        double PawSize { get; set; }
        string SocialBehavior { get; set; }

    }
}
