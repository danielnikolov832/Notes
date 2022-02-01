namespace NotesLibrary
{
    public class NamedNoteCollection : NoteCollection
    {
        public string? getset_name {get; set;}

        public NamedNoteCollection(string name)
        {
            getset_name = name;
        }
    }
}