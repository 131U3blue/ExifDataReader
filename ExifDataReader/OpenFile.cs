using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ExifDataReader
{
    class OpenFile
    {
        public static byte[] GetByteStream(string fileLocation)
        {
            try
            {
                var fileDataAsBytes = File.ReadAllBytes(fileLocation);
                if (fileLocation.ToLower() != "quit")
                {
                    return fileDataAsBytes;
                }
                return null;
            }
            // Catch file name error/missing file. Displays error message and starts loop again
            catch (IOException)
            {
                Console.WriteLine("File Not Found");
                fileLocation = string.Empty;
                return null;
            }
            // Catch blank entry. Displays error message and starts loop again
            catch (ArgumentException)
            {
                Console.WriteLine("\n\nPlease give a valid file location or type \"quit\" to exit.\n\n");
                return null;
            }
        }
    }
}
