using System.ComponentModel;

namespace NotesLibrary
{
    public class NamedNoteCollection<TNote> : NoteCollection<TNote> where TNote : Note
    {
        public string? get_name {get; protected set;}

        public NamedNoteCollection(string name)
        {
            get_name = name;
        }
    }
}