using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BusinessLogicLibrary.Models;

public class Note
{
    public string getset_name { get; set; }
    public string get_TypeName { get; init; }

    internal bool isTypeGenerated { get; init; }

    public ObservableCollection<IValueContainerNonGeneric> get_properties { get; init; }
    = new ObservableCollection<IValueContainerNonGeneric>();

    public ObservableCollection<Note> get_children {get; init;}

    public Note(string name, string typeName = "")
    {
        getset_name = name;
        get_TypeName = typeName;

        if (typeName != null)
        {
            isTypeGenerated = true;
        }

        get_children = new ObservableCollection<Note>();
    }

    public Note(string name, IEnumerable<Note> children, string typeName = "Normal")
    {
        getset_name = name;
        get_TypeName = typeName;

        if (typeName != null)
        {
            isTypeGenerated = true;
        }

        get_children = new ObservableCollection<Note>(children);
    }
}