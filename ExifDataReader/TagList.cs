using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExifDataReader
{
    class TagList
    {
        public static string GetTagName(Span<byte> tag)
        {
            var TagNames = new Dictionary<byte[], string> {
                { new byte[] { 0x00, 0xfe }, "New Subfile Type" },
                { new byte[] { 0x00, 0xff }, "Subfile Type" },
                { new byte[] { 0x01, 0x0e }, "Image Description" },
                { new byte[] { 0x01, 0x0f }, "Make" },
                { new byte[] { 0x01, 0x10 }, "Model" },
                { new byte[] { 0x01, 0x12 }, "Orientation" },
                { new byte[] { 0x01, 0x1a }, "X Resolution" },
                { new byte[] { 0x01, 0x1b }, "Y Resolution" },
                { new byte[] { 0x01, 0x28 }, "Resolution Unit" },
                { new byte[] { 0x01, 0x31 }, "Software" },
                { new byte[] { 0x01, 0x32 }, "DateTime" },
                { new byte[] { 0x01, 0x3e }, "White Point" },
                { new byte[] { 0x01, 0x3f }, "Primary Chromaticities" },
                { new byte[] { 0x02, 0x11 }, "YCbCr Coefficients" },
                { new byte[] { 0x02, 0x12 }, "YCbCr Positioning" },
                { new byte[] { 0x02, 0x14 }, "Reference Black White" },
                { new byte[] { 0x01, 0x2d }, "Transfer Function" },
                { new byte[] { 0x01, 0x3b }, "Artist" },
                { new byte[] { 0x01, 0x3d }, "Predictor" },
                { new byte[] { 0x01, 0x42 }, "Tile Width" },
                { new byte[] { 0x01, 0x43 }, "Tile Length" },
                { new byte[] { 0x01, 0x44 }, "Tile Offsets" },
                { new byte[] { 0x01, 0x45 }, "Tile Byte Counts" },
                { new byte[] { 0x01, 0x4a }, "Sub IFDs" },
                { new byte[] { 0x01, 0x5b }, "JPEG Tables" },
                { new byte[] { 0x82, 0x8d }, "CFA Repeat Pattern Dim" },
                { new byte[] { 0x82, 0x8e }, "CFA Pattern" },
                { new byte[] { 0x82, 0x8f }, "Battery Level" },
                { new byte[] { 0x82, 0x98 }, "Copyright" },
                { new byte[] { 0x82, 0x9a }, "Exposure Time" },
                { new byte[] { 0x82, 0x9d }, "F Number" },
                { new byte[] { 0x83, 0xbb }, "IPTC/NAA" },
                { new byte[] { 0x87, 0x69 }, "EXIF Offset" },
                { new byte[] { 0x87, 0x73 }, "Inter Colour Profile" },
                { new byte[] { 0x88, 0x24 }, "Spectral Sensitivity" },
                { new byte[] { 0x88, 0x25 }, "GPS Info" },
                { new byte[] { 0x88, 0x28 }, "OECF" },
                { new byte[] { 0x88, 0x29 }, "Interlace" },
                { new byte[] { 0x88, 0x2a }, "Time Zone Offset" },
                { new byte[] { 0x88, 0x2b }, "Self Timer Mode" },
                { new byte[] { 0x88, 0x22 }, "Exposure Program" },
                { new byte[] { 0x88, 0x27 }, "ISO Speed Ratings" },
                { new byte[] { 0x90, 0x00 }, "EXIF Version" },
                { new byte[] { 0x90, 0x03 }, "DateTime Original" },
                { new byte[] { 0x90, 0x04 }, "DateTime Digitized" },
                { new byte[] { 0x91, 0x01 }, "Component Configuration" },
                { new byte[] { 0x91, 0x02 }, "Compressed Bits Per Pixel" },
                { new byte[] { 0x92, 0x01 }, "Shutter Speed" },
                { new byte[] { 0x92, 0x02 }, "Aperture Value" },
                { new byte[] { 0x92, 0x03 }, "Brightness Value" },
                { new byte[] { 0x92, 0x04 }, "Exposure Bias Value" },
                { new byte[] { 0x92, 0x05 }, "Max Aperture Value" },
                { new byte[] { 0x92, 0x06 }, "Subject Distance" },
                { new byte[] { 0x92, 0x07 }, "Metering Mode" },
                { new byte[] { 0x92, 0x08 }, "Light Source" },
                { new byte[] { 0x92, 0x09 }, "Flash" },
                { new byte[] { 0x92, 0x0a }, "Focal Length" },
                { new byte[] { 0x92, 0x0b }, "Flash Energy" },
                { new byte[] { 0x92, 0x0c }, "Spatial Frequency Response" },
                { new byte[] { 0x92, 0x0d }, "Noise" },
                { new byte[] { 0x92, 0x11 }, "Image Number" },
                { new byte[] { 0x92, 0x12 }, "Security Classification" },
                { new byte[] { 0x92, 0x13 }, "Image History" },
                { new byte[] { 0x92, 0x14 }, "Subject Location" },
                { new byte[] { 0x92, 0x15 }, "Exposure Index" },
                { new byte[] { 0x92, 0x16 }, "TIFF/EP Standard ID" },
                { new byte[] { 0x92, 0x7c }, "Maker Note" },
                { new byte[] { 0x92, 0x86 }, "User Comment" },
                { new byte[] { 0x92, 0x90 }, "Sub Sec Time" },
                { new byte[] { 0x92, 0x91 }, "Sub Sec Time Original" },
                { new byte[] { 0x92, 0x92 }, "Sub Sec Time Digitized" },
                { new byte[] { 0xa0, 0x00 }, "Flash Pix Version" },
                { new byte[] { 0xa0, 0x01 }, "Colour Space" },
                { new byte[] { 0xa0, 0x02 }, "EXIF Image Width" },
                { new byte[] { 0xa0, 0x03 }, "EXIF Image Height" },
                { new byte[] { 0xa0, 0x04 }, "Related Sound File" },
                { new byte[] { 0xa0, 0x05 }, "EXIF Interoperability Offset" },
                { new byte[] { 0xa2, 0x0b }, "Flash Energy" },
                { new byte[] { 0xa2, 0x0c }, "Spatial Frequency Response" },
                { new byte[] { 0xa2, 0x0e }, "Focal Place X Resolution" },
                { new byte[] { 0xa2, 0x0f }, "Focal Place Y Resolution" },
                { new byte[] { 0xa2, 0x10 }, "Focal Plan Resolution Unit" },
                { new byte[] { 0xa2, 0x17 }, "Sensing Method" },
                { new byte[] { 0xa2, 0x14 }, "Subject Location" },
                { new byte[] { 0xa2, 0x15 }, "Exposure Index" },
                { new byte[] { 0xa3, 0x00 }, "File Source" },
                { new byte[] { 0xa3, 0x01 }, "Scene Type" },
                { new byte[] { 0xa3, 0x02 }, "CFA Pattern" }
            };
            foreach (var entry in TagNames) {
                if (tag.SequenceEqual(entry.Key)) {
                    return entry.Value;
                }
            }
            return "Unknown Tag";
        }
    }
}
