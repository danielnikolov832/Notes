using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUI.Mediators;
using static WPFUI.Mediators.Publications.SettingsVMPublications;

namespace WPFUI.VVM.Settings;
public class SettingsViewModel : ObservableObject
{
    public SettingsViewModel()
    {
    }

    private Color _primaryColor;

    public Color getset_primaryColor
    {
        get => _primaryColor;
        set
        {
            _primaryColor = value;
            OnPropertyChanged();

            StaticMediators.get_currentPublisherForVMs.Publish(new PrimaryColorChangedPublication(_primaryColor));
        }
    }

    private Color _backgroundColor;

    public Color getset_backgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            OnPropertyChanged();

            StaticMediators.get_currentPublisherForVMs.Publish(new BackgroundColorChangedPublication(_backgroundColor));
        }
    }
}
