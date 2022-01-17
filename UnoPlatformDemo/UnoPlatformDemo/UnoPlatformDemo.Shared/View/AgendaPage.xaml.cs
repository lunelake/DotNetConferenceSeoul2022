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
    public sealed partial class AgendaPage : Page, INotifyPropertyChanged
    {

        #region NotiBase

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private List<Agenda> _AgendaList;
        public List<Agenda> AgendaList
        {
            get { return _AgendaList; }
            set
            {
                if (_AgendaList == value)
                    return;

                _AgendaList = value;
                OnPropertyChanged();
            }
        }

        public AgendaPage()
        {
            this.InitializeComponent();

            this.Loaded += AgendaPage_Loaded;
        }

        private void AgendaPage_Loaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        public void Load()
        {
            AgendaList = new List<Agenda>()
            {
                new Agenda()
                {
                    Title = "Keynote",
                    Description = "Join Scott Hunter and team as they show you all the amazing things you can do with .NET 6.",
                    Time = "Tuesday, November 9, 2021 08:00 am",
                    Speaker = "David Fowler Scott Hunter Daniel RothMaria Naggaga Mads TorgersenMaddy Leger Montaquila"
                },
                new Agenda()
                {
                    Title = "What's new in C# 10",
                    Description = "C# 10 brings many improvements focused around enabling cleaner and simpler code in many scenarios.",
                    Time = "Tuesday, November 9, 2021 09:30 am",
                    Speaker = "Mads Torgersen Dustin Campbell"
                },
                new Agenda()
                {
                    Title = "Enterprise-grade Blazor apps with .NET 6",
                    Description = "Blazor in .NET 6 gives you the functionality you need to build real world apps of any size and complexity. In this session we'll look at the new Blazor features in .NET 6 for practical web app development. We'll look at the new support for Hot Reload, error boundaries, state preservation during prerendering, faster file uploads, query string handling, adding page metadata, and integrating Blazor components into existing JavaScript based apps.",
                    Time = "Tuesday, November 9, 2021 10:00 am",
                    Speaker = "Daniel Roth"
                },
                new Agenda()
                {
                    Title = "Introduction to .NET MAUI",
                    Description = ".NET MAUI is the best way to build cross platform mobile and desktop apps with .NET and C#. Join Maddy Leger, .NET MAUI Program Manager, to get a first look of .NET MAUI in .NET 6 and learn how you can start using it today.",
                    Time = "Tuesday, November 9, 2021 08:00 am",
                    Speaker = "Maddy Leger Montaquila"
                },
                new Agenda()
                {
                    Title = "What's New in F# 6",
                    Description = "F# 6 is all about modernizing and simplifying the experience of learning and using F#. This applies to the language design, library, performance and tooling. The most significant technical feature in F# 6 is a new mechanism for authoring async tasks that is easier and faster. List and arrays also got faster – in some cases 4 times faster! Language simplification includes more consistent rules for indentation, more implicit conversions, and letting you skip that period between an identifier and the square bracket of an index. You’ll find five new operations on collections and be able to access Keys and Values of a Map. Tooling is also improved with better performance, pipeline debugging, and better access to C# projects in your solution. Come hear more about F# 6!",
                    Time = "Tuesday, November 9, 2021 11:00 am",
                    Speaker = "Kathleen Dollard Don Syme"
                },
                                new Agenda()
                {
                    Title = "Keynote",
                    Description = "Join Scott Hunter and team as they show you all the amazing things you can do with .NET 6.",
                    Time = "Tuesday, November 9, 2021 08:00 am",
                    Speaker = "David Fowler Scott Hunter Daniel RothMaria Naggaga Mads TorgersenMaddy Leger Montaquila"
                },
                new Agenda()
                {
                    Title = "What's new in C# 10",
                    Description = "C# 10 brings many improvements focused around enabling cleaner and simpler code in many scenarios.",
                    Time = "Tuesday, November 9, 2021 09:30 am",
                    Speaker = "Mads Torgersen Dustin Campbell"
                },
                new Agenda()
                {
                    Title = "Enterprise-grade Blazor apps with .NET 6",
                    Description = "Blazor in .NET 6 gives you the functionality you need to build real world apps of any size and complexity. In this session we'll look at the new Blazor features in .NET 6 for practical web app development. We'll look at the new support for Hot Reload, error boundaries, state preservation during prerendering, faster file uploads, query string handling, adding page metadata, and integrating Blazor components into existing JavaScript based apps.",
                    Time = "Tuesday, November 9, 2021 10:00 am",
                    Speaker = "Daniel Roth"
                },
                new Agenda()
                {
                    Title = "Introduction to .NET MAUI",
                    Description = ".NET MAUI is the best way to build cross platform mobile and desktop apps with .NET and C#. Join Maddy Leger, .NET MAUI Program Manager, to get a first look of .NET MAUI in .NET 6 and learn how you can start using it today.",
                    Time = "Tuesday, November 9, 2021 08:00 am",
                    Speaker = "Maddy Leger Montaquila"
                },
                new Agenda()
                {
                    Title = "What's New in F# 6",
                    Description = "F# 6 is all about modernizing and simplifying the experience of learning and using F#. This applies to the language design, library, performance and tooling. The most significant technical feature in F# 6 is a new mechanism for authoring async tasks that is easier and faster. List and arrays also got faster – in some cases 4 times faster! Language simplification includes more consistent rules for indentation, more implicit conversions, and letting you skip that period between an identifier and the square bracket of an index. You’ll find five new operations on collections and be able to access Keys and Values of a Map. Tooling is also improved with better performance, pipeline debugging, and better access to C# projects in your solution. Come hear more about F# 6!",
                    Time = "Tuesday, November 9, 2021 11:00 am",
                    Speaker = "Kathleen Dollard Don Syme"
                },
            };
        }


        public void ItemClicked(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is Agenda agenda))
                return;

            MainPage.Current.Frame.Navigate(typeof(AgendaDetailPage), agenda);
        }

    }

    public class Agenda : INotifyPropertyChanged
    {
        #region NotiBase

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                if (_Title == value)
                    return;

                _Title = value;
                OnPropertyChanged();
            }
        }

        private string _Time;
        public string Time
        {
            get { return _Time; }
            set
            {
                if (_Time == value)
                    return;

                _Time = value;
                OnPropertyChanged();
            }
        }


        private string _Speaker;
        public string Speaker
        {
            get { return _Speaker; }
            set
            {
                if (_Speaker == value)
                    return;

                _Speaker = value;
                OnPropertyChanged();
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description == value)
                    return;

                _Description = value;
                OnPropertyChanged();
            }
        }

    }
}
