using System.Windows;
using Apd.Desktop.Messaging;
using GalaSoft.MvvmLight;

namespace Apd.Desktop.ViewModel {
    public class MainViewModel : ViewModelBase {
        public MainViewModel() {
            this.RegisterForMessages();
        }

        private void RegisterForMessages() {
            this.MessengerInstance.Register<ApiRequestError>(this,
                msg => {
                    MessageBox.Show(Application.Current.MainWindow, "Please check API address and try again.",
                        "API Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
        }
    }
}