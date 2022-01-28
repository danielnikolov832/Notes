using System.ComponentModel;

namespace NotesLibrary
{
    // The base form of a note, which is a holder of text information
    public class Note : INotifyPropertyChanged
    {
        private string name;
        public string getset_name
        {
            get => name;
            set 
            {
                name = value;
                OnPropertyChanged(nameof(getset_name));
            }
        }

        private string text;
        public string getset_text
        {
            get => text;
            set 
            {
                text = value;
                OnPropertyChanged(nameof(getset_text));
            }
        }

        private Note? parentNote;
        public Note? get_parentNote
        {
            get => parentNote;
            private set 
            {
                parentNote = value;
                OnPropertyChanged(nameof(get_parentNote));
            }
        }
                
        public NoteCollection get_subNotes { get; init; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Note(string name, string text = "")
        {
            this.name = name;
            this.text = text;

            get_subNotes = new NoteCollection();

            SetSubNotesEvents();
        }

        public Note(string name, IEnumerable<Note> subNotes, string text = "")
        {
            this.name = name;
            this.text = text;

            get_subNotes = new NoteCollection();

            SetSubNotesEvents();

            foreach (Note subNote in subNotes)
            {
                get_subNotes.Add(subNote);
            }
        }

        private void SetSubNotesEvents()
        {
            get_subNotes.OnBeforeInsertItem += get_SubNotes_NoteCollection_OnBeforeInsertOrSetItem;
            get_subNotes.OnBeforeSetItem += get_SubNotes_NoteCollection_OnBeforeInsertOrSetItem;
            get_subNotes.OnBeforeRemoveItem += get_SubNotes_NoteCollection_OnBeforeRemoveItem;
        }

        protected virtual void get_SubNotes_NoteCollection_OnBeforeInsertOrSetItem(object? sender, Note e)
        {
            e.parentNote = this;
        }

        protected virtual void get_SubNotes_NoteCollection_OnBeforeRemoveItem(object? sender, Note e)
        {
            e.parentNote = null;
        }
    }
}