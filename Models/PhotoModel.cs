using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Maui.OCR;

namespace OCRApplication.Models
{
    public class PhotoModel
    {
        public string ImageName { get; set; }
        public ImageSource ImageUrl { get; set; }
        public bool Success { get; set; }
        public string AllText { get; set; }
        public string Elements { get; set; }
        public string Lines { get; set; }
        //public IList<OcrResult.OcrElement> Elements { get; set; } = new List<OcrResult.OcrElement>();
        //public IList<string> Lines { get; set; } = new List<string>();
        //public class OcrElement
        //{
        //    public string Text { get; set; }
        //    public float Confidence { get; set; }
        //    public int X { get; set; }
        //    public int Y { get; set; }
        //    public int Height { get; set; }
        //    public int Width { get; set; }
        //}

        public PhotoModel(string imageName, ImageSource imageUrl, bool success, string allText, string elements, string lines)
        {
            ImageName = imageName;
            ImageUrl = imageUrl;
            Success = success;
            AllText = allText;
            Elements = elements;
            Lines = lines;
        }
    }
}
