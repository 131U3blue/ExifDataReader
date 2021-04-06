using System;
using System.Text;
using FlickrNet;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace ExifDataReader {
    class Program {
        public static Encoding utf8converter = Encoding.UTF8;
        static async Task Main(string[] args) {
            var photoByteArrayList = await GetSearchResult(GetSearchCriteria());
            foreach (byte[] array in photoByteArrayList) {
                BeginParsing(array);
            }
        }
        public static void BeginParsing(byte[] array)
        {
            var byteSpan = new Span<byte>(array); //Surely any method that converts the Task<byte[]> into byte[] and then Span<byte[]> will HAVE to block the async code?
            var segmentList = new PossibleSegmentList();
            IEnumerable<object> parsedDataObjectList = GetSegmentMarkers(byteSpan, segmentList);
            AddParsedSegmentsToList(parsedDataObjectList);
        }
        public static PhotoSearchOptions GetSearchCriteria() {
            Console.WriteLine("Search Flickr for images relating to: ");
            return FlickrApi.FlickrSearchCriteria(Console.ReadLine());            
        }
        public static async Task<List<byte[]>> GetSearchResult(PhotoSearchOptions searchCriteria) {
            return await FlickrApi.GetTaskList(searchCriteria);
        }

        public static IEnumerable<object> GetSegmentMarkers(Span<byte> byteSpan, PossibleSegmentList parserList) {
            var parsedDataObjectList = new List<object>();
            int segmentStartIndex = 0;
            for (int i = 0; i < (byteSpan.Length - 3); i++) {
                foreach (IMainSegmentParser segment in parserList.InstantiatedList) {
                    if (segment.MatchesMarker(byteSpan[i..(i + 2)]) && segment.ExifValidator(byteSpan[(i + 4)..(i + 8)])) {
                        int segmentLength = segment.GetSegmentLength(byteSpan[(i + 2)..(i + 4)]);
                        int segmentEndIndex = segmentStartIndex + segmentLength;
                        object parsedDataObject = segment.ParseSegment(byteSpan[segmentStartIndex..(segmentEndIndex + 1)]);
                        Console.WriteLine($"Start point: {i}({segmentStartIndex}), End Point: {segmentEndIndex} Length: {segmentLength}");
                        parsedDataObjectList.Add(parsedDataObject);
                    }
                }
                segmentStartIndex++;
            }
            return parsedDataObjectList;
        }
        public static List<object> AddParsedSegmentsToList(IEnumerable<object> parsedDataObjectList) {
            var listOfSegments = new List<object>();
            foreach (object parsedDataObject in parsedDataObjectList) {
                if (parsedDataObject is APP1Data app1Data) {
                    DisplayBasicInfo(app1Data);
                    listOfSegments.Add(parsedDataObject);
                }
            }
            return listOfSegments;
        }
        public static void DisplayBasicInfo(APP1Data app1Data) {
            Console.WriteLine(
                $"APP1 \n\t" +
                $"Is Big Endian: {app1Data.IsBigEndian}\n\t" +
                $"IFD Directories: {app1Data.IFDOverview.AmountOfDirectories}\n\t" +
                $"IFD Offset: {app1Data.Offset}\n\t");
            foreach (var ifd in app1Data.IFDData) {
                Console.WriteLine(" + + + THIS IS A MAIN IFD + + + ");
                DisplayTagInfo(ifd);
                foreach (var subIfd in ifd.SubIFDData) {
                    Console.WriteLine(" *-*-* THIS IS A SUB-IFD *-*-* ");
                    DisplayTagInfo(subIfd);
                }
            }
        }
        public static void DisplayTagInfo(Markers.APPnMarkers.IFDTagParser tag)
        {
            Console.WriteLine($"\n\t\tTag: {tag.TagName} ({tag.DirectoryTagNum[0]:x} {tag.DirectoryTagNum[1]:x})");
            Console.WriteLine($"\t\tExpected Format: {tag.DataFormatIndicator}");
            if (tag.ParsedData.GetType() == typeof(Rational)) { Console.WriteLine($"\t\tTag Value: " +
                $"{((Rational)tag.ParsedData).Numerator}/{((Rational)tag.ParsedData).Denominator} ({tag.ParsedData.GetType()})"); }
            else { Console.WriteLine($"\t\tTag value: {tag.ParsedData} ({tag.ParsedData.GetType()})"); }
        }
    }
}
//var byteArray = OpenFile.GetByteStream("D:\\Seb\\50817142731_dfc154ea75_o.jpg");
//var byteArray = OpenFile.GetByteStream("D:\\Seb\\31584653804_1c0003bbfc_o.jpg");