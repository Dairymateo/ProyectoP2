using Camera.MAUI;
using CommunityToolkit.Mvvm.Messaging;
using ProyectoP2.Utilities;



namespace ProyectoP2.Views
{
    public partial class BarcodePage : ContentPage
    {
        public BarcodePage()
        {
            InitializeComponent();
            cameraView.BarCodeOptions = new BarcodeDecodeOptions
            {
                TryHarder = true,
                PossibleFormats = MapBarcodeFormats(new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.All_1D }) // <-- Llamada al método de mapeo
            };
        }

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            if (cameraView.Cameras.Count > 0)
            {
                cameraView.Camera = cameraView.Cameras.First();
                MainThread.BeginInvokeOnMainThread(new Action(async () =>
                {
                    await cameraView.StopCameraAsync();
                    await cameraView.StartCameraAsync();
                }));
            }
        }

        private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
        {
            ProyectoP2.Utilities.BarcodeResult barcodeResult = new ProyectoP2.Utilities.BarcodeResult { BarcodeValue = args.Result[0].Text }; 
            WeakReferenceMessenger.Default.Send(new BarcodeScannedMessage(barcodeResult));


            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PopModalAsync();
            });
        }

        // Método para mapear formatos de ZXing a Camera.MAUI
        private List<BarcodeFormat> MapBarcodeFormats(List<ZXing.BarcodeFormat> zxingFormats)
        {
            var cameraMauiFormats = new List<BarcodeFormat>();
            foreach (var format in zxingFormats)
            {
                cameraMauiFormats.Add((BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), format.ToString()));
            }
            return cameraMauiFormats;
        }
    }
}
