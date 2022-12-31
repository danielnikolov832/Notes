using Syncfusion.SfSkinManager;
using Syncfusion.Themes.Office2019Colorful.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using System.Text;

using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Syncfusion.Themes.MaterialDark.WPF;
using Syncfusion.Themes.MaterialLight.WPF;
using Syncfusion.Themes.VisualStudio2015.WPF;

namespace WPFUI.VVM.MainWindow;

/// <summary>
/// Interaction logic for MainWindowVMs.xaml
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly WPFUI.VVM.HomeView.HomeView _homeView = new();
    private readonly WPFUI.VVM.Settings.Settings _settingsView = new();
    private readonly WPFUI.VVM.TypeEditorView.TypeEditorView _typeEditorView = new();

    private readonly MainWindowViewModel _mainWindowViewModel = new();

    private Office2019ColorfulThemeSettings _office2019ColorfulThemeSettings = new()
    {
        
    };

    public MainWindow()
    {
        InitializeComponent();

        //MaterialDarkThemeSettings materialDarkThemeSettings = new()
        //{
        //};

        //MaterialLightThemeSettings materialLightThemeSettings = new()
        //{
        //};

        SfSkinManager.RegisterThemeSettings("Office2019Colorful", _office2019ColorfulThemeSettings);

        SfSkinManager.SetTheme(this, new Theme("Office2019Colorful"));

        //SfSkinManager.RegisterThemeSettings("MaterialDark", materialDarkThemeSettings);

        //SfSkinManager.SetTheme(this, new Theme("MaterialDark"));

        //SfSkinManager.RegisterThemeSettings("MaterialLight", materialDarkThemeSettings);

        PageView.Content = _homeView;

        DataContext = _mainWindowViewModel;

        _mainWindowViewModel.PropertyChanged += VM_PropertyChanged_ChangeView;
        _mainWindowViewModel.PropertyChanged += VM_PropertyChanged_ChangePrimaryColorOfTheme;
    }

    private void VM_PropertyChanged_ChangeView(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.getset_differentWindowsEnum))
        {
            ChangePage(_mainWindowViewModel.getset_differentWindowsEnum);
        }
    }

    private void ChangePage(DifferentWindowsEnum differentWindowsEnum)
    {
        switch (differentWindowsEnum)
        {
            case DifferentWindowsEnum.MainWindow: PageView.Content = _homeView;
                break;
            case DifferentWindowsEnum.Settings: PageView.Content = _settingsView;
                break;
            case DifferentWindowsEnum.TypeEditor: PageView.Content = _typeEditorView;
                break;
        }
    }

    private void VM_PropertyChanged_ChangePrimaryColorOfTheme(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.getset_primaryColor))
        {
            _office2019ColorfulThemeSettings = new()
            {
                PrimaryBackground = new SolidColorBrush(_mainWindowViewModel.getset_primaryColor)
            };

            SfSkinManager.RegisterThemeSettings("Office2019Colorful", _office2019ColorfulThemeSettings);

            SfSkinManager.SetTheme(this, new Theme("Office2019Colorful"));
        }
    }
}
