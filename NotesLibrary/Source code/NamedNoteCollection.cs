namespace NotesLibrary
{
    public class NamedNoteCollection : NoteCollection
    {
        public string? get_name {get; protected set;}

        public NamedNoteCollection(string name)
        {
            get_name = name;
        }
    }
}