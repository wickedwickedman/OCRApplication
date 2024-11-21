using OCRApplication.Models;
using Plugin.Maui.OCR;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OCRApplication.ViewModels
{
    public class PhotoUploadViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<PhotoModel> image = new();
        private bool isLoading;
        private IOcrService ocr = OcrPlugin.Default;

        public ICommand PhotoUploadCommand { get; set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<PhotoModel> Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public PhotoUploadViewModel()
        {
            PhotoUploadCommand = new Command(async () => await PhotoUpload());
        }

        public async Task PhotoUpload()
        {
            var items = await PickPhoto();
            if (items is not null)
            {
                foreach (var item in items)
                {
                    if (item is FileResult)
                    {
                        var stream = await item.OpenReadAsync();
                        var tempImage = ImageSource.FromStream(() => stream);
                        var result = await ProcessPhoto(item);

                        var Elements = result.Elements;
                        var Lines = result.Lines;
                        StringBuilder elementsBuilder = new();
                        StringBuilder lineBuilder = new();
                        foreach (var element in Elements)
                        {
                            elementsBuilder.AppendLine(element.Text);
                        }
                        foreach (var line in Lines)
                        {
                            lineBuilder.AppendLine(line);
                        }
                        image.Add(new PhotoModel(item.FullPath, tempImage, result.Success, result.AllText, elementsBuilder.ToString(), lineBuilder.ToString()));
                        OnPropertyChanged(nameof(image));
                    }
                }
            }
        }

        public async Task<IEnumerable<FileResult>> PickPhoto()
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "UUType.Image" } },
                    { DevicePlatform.Android, new[] { "image/*" } },
                    { DevicePlatform.WinUI, new[] { ".jpg", ".jpeg", ".png", ".bmp" } },
                    { DevicePlatform.macOS, new[] { ".jpg", ".jpeg", ".png", ".bmp" } },
                });

            PickOptions options = new()
            {
                PickerTitle = "Upload File",
                FileTypes = customFileType,
            };
            try
            {
                var result = await FilePicker.Default.PickMultipleAsync(options);
                //if (result != null)
                //{
                //    foreach (var file in result)
                //    {
                //        var stream = await file.OpenReadAsync();
                //        var image = ImageSource.FromStream(() => stream);
                //    }
                //}
                return result!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null!;
        }

        private async Task<OcrResult> ProcessPhoto(FileResult photo)
        {
            // Open a stream to the photo
            using var sourceStream = await photo.OpenReadAsync();

            // Create a byte array to hold the image data
            var imageData = new byte[sourceStream.Length];

            // Read the stream into the byte array
            await sourceStream.ReadAsync(imageData);

            // Set options
            var options = new OcrOptions.Builder()
            //.SetLanguage("en-US")
            .SetLanguage("ko-KR")
            .SetLanguage("en-US")
            .SetTryHard(true)
            //.AddPatternConfig(new OcrPatternConfig(@"\d{10}"))
            .Build();

            // Process the image data using the OCR service
            return await ocr.RecognizeTextAsync(imageData, options);
        }
    }
}
