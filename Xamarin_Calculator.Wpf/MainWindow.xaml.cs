using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace Xamarin_Calculator.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();
            Forms.Init();
            LoadApplication(new Xamarin_Calculator.App(new WpfInitializer()));
        }
    }

    public class WpfInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {

        }
    }

}
