using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UnoPlatformDemo.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AgendaDetailPage : Page, INotifyPropertyChanged
    {

        #region NotiBase

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private Agenda _Agenda;
        public Agenda Agenda
        {
            get { return _Agenda; }
            set
            {
                if (_Agenda == value)
                    return;

                _Agenda = value;
                OnPropertyChanged();
            }
        }

        public AgendaDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var agenda = e.Parameter as Agenda;
            Agenda = agenda;
        }

        public void GoBack()
            => this.Frame.GoBack();
    }
}
