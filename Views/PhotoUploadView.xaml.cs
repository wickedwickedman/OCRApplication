namespace OCRApplication.Views;

public partial class PhotoUploadView : ContentPage
{
    private Color originalBackgroundColor;

	public PhotoUploadView()
	{
		InitializeComponent();
	}

    private void OnButtonPressed(object s, EventArgs e)
    {
        if (s is Button button)
        {
            originalBackgroundColor = button.BackgroundColor;
            button.BackgroundColor = Colors.LightGray;
        }
    }

    private void OnButtonReleased(object s, EventArgs e)
    {
        if (s is Button button)
        {
            button.BackgroundColor = originalBackgroundColor;
        }
    }
}