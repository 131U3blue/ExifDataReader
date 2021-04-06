using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrNet;
using System.Configuration;
using System.Net.Http;

namespace ExifDataReader
{
    class FlickrApi
    {
        const string apiKey = "d489f3c69e5a6eefbee74b3b68f549e0";
        public static PhotoSearchOptions FlickrSearchCriteria(string searchTerm)
        {
 
            var searchCriteria = new PhotoSearchOptions {
                Tags = searchTerm,
                PerPage = 20,
                Page = 1
            };
            return searchCriteria;
        }

        public static async Task<List<byte[]>> GetTaskList(PhotoSearchOptions searchCriteria)
        {
            var imageDownloadTasks = new List<byte[]>();
            var taskList = new List<Task<byte[]>>();
            var flickr = new Flickr(apiKey);
            PhotoCollection newCollection = flickr.PhotosSearch(searchCriteria);
            foreach (Photo photo in newCollection)
            {
                taskList.Add(DownloadImage(photo));
            }
            foreach (var task in taskList) {
                imageDownloadTasks.Add(await task);
            }
 //           IEnumerable<Task<byte[]>> downloadableTaskQuery =
 //               from imageDownloadTask in imageDownloadTasks
 //               select GetByteArray(imageDownloadTask);

            return imageDownloadTasks;
        }
        public static async Task<byte[]> DownloadImage(Photo photo)
        {
            Console.WriteLine("\n\nSTARTING DOWNLOAD\n\n");
            var client = new HttpClient();
            var request = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{photo.LargeUrl}"),
                Headers =
                {
                    { "x-rapidapi-key", apiKey
                    },
                    { "x-rapidapi-host", "FlickrdidenkoradionV1.p.rapidapi.com"
                    },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "apiKey", "d489f3c69e5a6eefbee74b3b68f549e0" },
                    { "userId", "8eebdd4b2db9901a" },
                }),
            };
            var response = await client.GetByteArrayAsync(request.RequestUri);
            Console.WriteLine("AWAIT HERE");
            return response;
        }
        public static void CreateTempFolder(string fileName)
        {
            string folderLocation = @"C:\Users\Seb\source\repos\ExifDataReader";
            string newFolder = Path.Combine(folderLocation, "ImagesTemp");
            if (!Directory.Exists(newFolder)) {
                Directory.Delete(newFolder);
            }
            Directory.CreateDirectory(newFolder);
            string fullPath = Path.Combine(newFolder, fileName);
            if (!File.Exists(fullPath)) {
                File.Delete(fullPath);
            }
            File.Create(fullPath);
        }



    }
}
