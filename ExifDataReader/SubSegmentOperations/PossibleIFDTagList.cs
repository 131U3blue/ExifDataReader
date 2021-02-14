using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace ExifDataReader.Parsers {
    //Static constructor here - consolidate this with ISegmentParser as well.
    class PossibleIFDTagList {
        public List<ISubSegmentParser> InstantiatedList { get; }
        public PossibleIFDTagList() {
            InstantiatedList = Assembly.GetAssembly(typeof(PossibleIFDTagList))
                .GetTypes()
                .Where(t => typeof(ISubSegmentParser).IsAssignableFrom(t))
                .Where(t => (t.IsClass || t.IsValueType) && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t))
                .Cast<ISubSegmentParser>()
                .ToList();
        }
    }
}
