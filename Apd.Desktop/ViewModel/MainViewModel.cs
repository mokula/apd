using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RestSharp;

namespace Apd.Desktop.ViewModel {
    public class MainViewModel : ViewModelBase {
        private RelayCommand checkConnection;

        public RelayCommand CheckConnection {
            get {
                if (this.checkConnection == null) {
                    this.checkConnection = new RelayCommand(() => {
                        var client = new RestClient("http://localhost:61318/api/");
                        var request = new RestRequest("contact/all", Method.GET);
                        var response = client.Execute(request);
                        var content = response.Content;
                    });
                }

                return this.checkConnection;
            }            
        }

        public MainViewModel() {
        }
    }
}