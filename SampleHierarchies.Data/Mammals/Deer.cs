using SampleHierarchies.Interfaces.Data;
using SampleHierarchies.Interfaces.Data.Mammals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Data.Mammals
{
    public class Deer : MammalBase, IDeer
    {
        #region Public Methods
        public override void Display()
        {
            Console.WriteLine($"My name is: {Name}, my age is: {Age}. I have {AntlerCount} antler(s), {CoatColor} color of coat and can run {Speed} km/h");
        }
        public override void Copy(IAnimal animal)
        {
            if (animal is IDeer ad)
            {
                base.Copy(animal);
                AntlerCount = ad.AntlerCount;
                LengthOfAntlers = ad.LengthOfAntlers;
                Speed = ad.Speed;
                CoatColor = ad.CoatColor;
                Habitat = ad.Habitat;
            }
        }
        #endregion // Public Methods

        #region Ctors And Properties
        public int AntlerCount { get; set; }
        public double LengthOfAntlers { get; set; }
        public double Speed { get; set; }
        public string CoatColor { get; set; }
        public string Habitat { get; set; }

        public Deer(string name, int age, int antlerCount, double lengthOfAntlers, double speed, string coatColor, string habitat) : base(name, age, MammalSpecies.Deer)
        {
            AntlerCount= antlerCount;
            LengthOfAntlers= lengthOfAntlers;
            Speed= speed;
            CoatColor = coatColor;
            Habitat= habitat;
        }
        #endregion
    }
}
