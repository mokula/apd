using Apd.Desktop.Service;
using GalaSoft.MvvmLight;

namespace Apd.Desktop.ViewModel {
    public class ApiSettingsViewModel : ViewModelBase {
        private IRestApi restApi;

        public string ApiAddress {
            get => this.restApi.ApiAddress;
            set {
                if (this.restApi.ApiAddress == value)
                    return;

                this.restApi.ApiAddress = value;
                this.RaisePropertyChanged(nameof(this.ApiAddress));
            }
        }

        public ApiSettingsViewModel(IRestApi restApi) {
            this.restApi = restApi;
        }
    }
}