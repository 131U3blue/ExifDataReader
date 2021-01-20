using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ExifDataReader
{
    class PossibleSegmentList
    {
        public List<ISegmentParser> InstantiatedList { get; }
        public PossibleSegmentList()
        {
            InstantiatedList = Assembly.GetAssembly(typeof(PossibleSegmentList)) //Checks ENTIRE assembly for ALL types - Any type within the Assembly as this argument needs to point to the correct assembly, 
                .GetTypes()                                                      // and ANY type within the assembly can do this!
                .Where(t => typeof(ISegmentParser).IsAssignableFrom(t))     //Get Types that ISegmentParser can be assigned to.
                .Where(t => (t.IsClass || t.IsValueType) && !t.IsAbstract)  //And that are class OR value types which are also NOT abstract.
                .Select(t => Activator.CreateInstance(t))                   //Take those types and create an instance of them - hence filtering classes and NOT abstract!
                .Cast<ISegmentParser>()                                     //Converts those types to ISegmentParse (as we know they can be from .IsAssignableFrom(t))
                .ToList();                                                  //Add to list
        }
    }

   
}
