using System;
using System.Collections.Generic;
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

namespace WPFUI.VVM.PropertyListView;

/// <summary>
/// Interaction logic for PropertyListView.xaml
/// </summary>
public partial class PropertyListView : UserControl
{
    public PropertyListView ()
    {
        InitializeComponent();

        TypeCreateButton.Click += TypeCreateButton_Click;
    }

    private void TypeCreateButton_Click (object sender, RoutedEventArgs e)
    {
        OnTypeCreateButtonClick();
    }

    public static readonly RoutedEvent TypeCreateButtonClickEvent = EventManager.RegisterRoutedEvent(
        name: "TypeCreateButtonClick",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(PropertyListView));

    // Provide CLR accessors for adding and removing an event handler.

#pragma warning disable RCS1159 // Use EventHandler<T>.
    public event RoutedEventHandler TypeCreateButtonClick
#pragma warning restore RCS1159 // Use EventHandler<T>.
    {
        add { AddHandler(TypeCreateButtonClickEvent, value); }
        remove { RemoveHandler(TypeCreateButtonClickEvent, value); }
    }

    private void OnTypeCreateButtonClick ()
    {
        RaiseEvent(new(TypeCreateButtonClickEvent));
    }
}
