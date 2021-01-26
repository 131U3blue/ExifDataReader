using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace ExifDataReader
{
    class Program
    {
        public static Encoding utf8converter = Encoding.UTF8;
        public static int bytePosition = 0;
        public static int sectionNumber = 0;
        static void Main(string[] args)
        {
            var byteArray = OpenFile.GetByteStream("D:\\Seb\\31584653804_1c0003bbfc_o.jpg");
            var byteSpan = new Span<byte>(byteArray);
            var segmentList = new PossibleSegmentList();
            IEnumerable<object> parsedDataObjectList = GetSegmentMarkers(byteSpan, segmentList);
            AddParsedSegmentsToList(parsedDataObjectList);
        }


        //ICollection with yield return - this method will run until the user's condition is met. E.g. GetSegmentMarkers.OfType<APP1Data>().First()
        //ICollections and IEnumerables can be LAZILY ITERATED and can also be easily changed to e.g. hash sets
        public static IEnumerable<object> GetSegmentMarkers(Span<byte> byteSpan, PossibleSegmentList parserList)
        {
            var parsedDataObjectList = new List<object>();
            for (int i = 0; i < (byteSpan.Length - 3); i++)
            {
                foreach (ISegmentParser segment in parserList.InstantiatedList)
                {
                    if (segment.MatchesMarker(byteSpan[i..(i + 2)]) && segment.ExifValidator(byteSpan[(i + 4)..(i + 8)]))
                    {

                        sectionNumber++;
                        int segmentLength = segment.GetSegmentLength(byteSpan[(i + 2)..(i + 4)]);
                        int segmentStartIndex = bytePosition;
                        int segmentEndIndex = bytePosition + segmentLength;
                        object parsedDataObject = segment.ParseSegment(byteSpan[segmentStartIndex..segmentEndIndex]);
                        Console.WriteLine($"Start point: {i}({bytePosition}), End Point: {segmentEndIndex} Length:{segmentLength}");
                        parsedDataObjectList.Add(parsedDataObject);
                    }
                }
                bytePosition++;
            }
            return parsedDataObjectList;
        }
        public static List<object> AddParsedSegmentsToList(IEnumerable<object> parsedDataObjectList)
        {
            var listOfSegments = new List<object>();
            foreach (object parsedDataObject in parsedDataObjectList)
            {
                //if (parsedDataObject is APP0Data app0Data)
                //{
                //    Console.WriteLine($"APP0 Created: {app0Data.IFDData.CreationDate}, DPI: {app0Data.IFDData.DPI}");
                //    listOfSegments.Add(parsedDataObject);
                //}
                if (parsedDataObject is APP1Data app1Data)
                {
                    Console.WriteLine(
                        $"APP1 \n\t" +
                        $"Created: {app1Data.IFDData.CreationDate}\n\t" +
                        $"DPI: {app1Data.IFDData.DPI}\n\t" +
                        $"Is Big Endian: {app1Data.IsBigEndian}\n\t" +
                        $"IFD Directories: {app1Data.IFDData.AmountOfDirectories}\n\t" +
                        $"IFD Offset: {app1Data.Offset}\n\t"
                        );
                    listOfSegments.Add(parsedDataObject);

                }
            }
            return listOfSegments;
        }
    }
}
