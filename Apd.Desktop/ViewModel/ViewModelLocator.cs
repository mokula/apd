
using System.Configuration;
using Apd.Common.Container;
using Apd.Desktop.Service;
using GalaSoft.MvvmLight.Messaging;

namespace Apd.Desktop.ViewModel {
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator {
        private TinyIoCContainer container;
        
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator() {
            this.container = new TinyIoCContainer();
            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            //this.container.Register<>()
            this.container.Register<IContainer>(this.container);
            this.container.Register(Messenger.Default);
            this.container.Register<IRestClientFactory, RestClientFactory>();
            this.container.Register<IContacts, Contacts>();

            string apiAddress = ConfigurationManager.AppSettings["api_address"] ?? "";
            
            this.container.Register<MainViewModel>();
            this.container.Register<ApiSettingsViewModel>();
            this.container.Register<ContactsViewModel>();
            this.container.Register<ContactViewModel>();
            this.container.Register<ShieldViewModel>();
            this.container.Register<IRestApi, RestApi>(new RestApi(this.container.Resolve<IRestClientFactory>(), this.container.Resolve<IMessenger>(), apiAddress));
        }

        public MainViewModel Main => this.container.Resolve<MainViewModel>();
        public ApiSettingsViewModel ApiSettings => this.container.Resolve<ApiSettingsViewModel>();
        public ContactsViewModel Contacts => this.container.Resolve<ContactsViewModel>();
        public ShieldViewModel Shield => this.container.Resolve<ShieldViewModel>();

        public static void Cleanup() {
            // TODO Clear the ViewModels
        }
    }
}