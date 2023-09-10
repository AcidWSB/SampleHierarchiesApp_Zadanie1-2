using SampleHierarchies.Interfaces.Data;
using SampleHierarchies.Interfaces.Data.Mammals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHierarchies.Data.Mammals
{
    public class Cat : MammalBase, ICat
    {
        #region Public Methods
        public override void Display()
        {
            Console.WriteLine($"My name is: {Name}, my age is: {Age}. My hobby is {Hobbies} and i {Personality}");
        }
        public override void Copy(IAnimal animal)
        {
            if (animal is ICat ad)
            {
                base.Copy(animal);
                TailLength = ad.TailLength;
                Personality = ad.Personality;
                Hobbies = ad.Hobbies;
                FavoriteSpot = ad.FavoriteSpot;
            }
        }
        #endregion

        #region Ctors And Properties
        public double TailLength { get; set; }
        public string Personality { get; set; }
        public string Hobbies { get; set; }
        public string FavoriteSpot { get; set; }

        public Cat(string name, int age, double tailLength, string personality, string hobbies, string favoriteSpot) : base(name, age, MammalSpecies.Cat)
        {
            TailLength = tailLength;
            Personality = personality;
            Hobbies = hobbies;
            FavoriteSpot = favoriteSpot;
        }
        #endregion
    }
}
