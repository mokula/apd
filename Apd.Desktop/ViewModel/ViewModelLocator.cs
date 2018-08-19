
using Apd.Common.Container;

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

            this.container.Register<MainViewModel>();
            this.container.Register<ApiSettingsViewModel>();
            this.container.Register<ContactsViewModel>();
        }

        public MainViewModel Main => this.container.Resolve<MainViewModel>();
        public ApiSettingsViewModel ApiSettings => this.container.Resolve<ApiSettingsViewModel>();
        public ContactsViewModel Contacts => this.container.Resolve<ContactsViewModel>();

        public static void Cleanup() {
            // TODO Clear the ViewModels
        }
    }
}