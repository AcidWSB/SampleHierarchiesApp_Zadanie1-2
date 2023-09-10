using SampleHierarchies.Interfaces.Data;
using SampleHierarchies.Interfaces.Data.Mammals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Data.Mammals
{
    public class Panda : MammalBase, IPanda
    {
        #region Public Methods
        public override void Display()
        {
            Console.WriteLine($"My name is: {Name}, my age is: {Age}. I'm {KindOf} panda.I have {SpotCount} spots on my body and {PawSize} cm paws");
        }
        public override void Copy(IAnimal animal)
        {
            if (animal is IPanda ad)
            {
                base.Copy(animal);
                KindOf = ad.KindOf;
                SpotCount = ad.SpotCount;
                PawSize = ad.PawSize;
                SocialBehavior = ad.SocialBehavior;
            }
        }
        #endregion // Public Methods

        #region Ctors And Properties
        public string KindOf { get; set; }
        public int SpotCount { get; set; }
        public double PawSize { get; set; }
        public string SocialBehavior { get; set; }

        public Panda(string name, int age, string kindOf, int spotCount, double pawSize,string socialBehavior) : base(name, age, MammalSpecies.Panda)
        {
            KindOf = kindOf;
            SpotCount = spotCount;
            PawSize = pawSize;
            SocialBehavior = socialBehavior;
        }
        #endregion
    }
}
