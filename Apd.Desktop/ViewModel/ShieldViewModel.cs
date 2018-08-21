using Apd.Desktop.Messaging;
using GalaSoft.MvvmLight;

namespace Apd.Desktop.ViewModel {
    public class ShieldViewModel: ViewModelBase {
        private bool active;

        public bool Active {
            get => active;
            set { 
                if (this.active == value)
                    return;
                
                this.active = value;
                this.RaisePropertyChanged(nameof(this.Active));
            }
        }


        public ShieldViewModel() {
            this.RegisterForMessages();
        }

        private void RegisterForMessages() {
            this.MessengerInstance.Register<ApiRequestStarted>(this, msg => this.Active = true);
            this.MessengerInstance.Register<ApiRequestEnded>(this, msg => this.Active = false);
            this.MessengerInstance.Register<ApiRequestError>(this, msg => this.Active = false);
        }
    }
}