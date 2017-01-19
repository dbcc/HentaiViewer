using System.Windows.Input;
using HentaiViewer.Common;
using HentaiViewer.Models;
using PropertyChanged;

namespace HentaiViewer.ViewModels {
    [ImplementPropertyChanged]
    public class SettingsViewModel {
        public SettingsViewModel() {
            SaveCommand = new ActionCommand(() => {
                SettingsController.Settings = Setting;
                SettingsController.Save();
            });
            Setting = SettingsController.Settings;
        }

        public SettingsModel Setting { get; set; }

        //public string IpbMemberId => SettingsController.Settings.IpbMemberId;

        //public string IpbPassHash => SettingsController.Settings.IpbPassHash;

        //public string IpbSessionId => SettingsController.Settings.IpbSessionId;

        //public string SearchQuery => SettingsController.Settings.SearchQuery;

        public ICommand SaveCommand { get; }
    }
}