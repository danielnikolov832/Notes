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

                
        private NoteCollection get_subNotes { get; init; }
        public IReadOnlyCollection<Note> get_subNotesAsReadOnly
        {
            get => (IReadOnlyCollection<Note>)get_subNotes;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Temp_GetEventInvocationListAsString()
        {
            string output = string.Empty;

            if (PropertyChanged != null)
            {
                foreach (Delegate _delegate in PropertyChanged.GetInvocationList())
                {
                    output += _delegate.Method.Name + "\n";
                }
            }
            
            return output;
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
            e.get_parentNote = this;
        }

        protected virtual void get_SubNotes_NoteCollection_OnBeforeRemoveItem(object? sender, Note e)
        {
            e.get_parentNote = null;
        }

        public void AddSubNote(Note subNote)
        {
            get_subNotes.Add(subNote);
        }

        public Note? GetSubNoteWithName(string name)
        {
            Note? output = get_subNotes.GetNoteWithName(name);

            return output;
        }

        public bool RemoveSubNote(Note subNote)
        {
            bool output = get_subNotes.Remove(subNote);

            return output;
        }

        public void RemoveSubNoteAtIndex(int index)
        {
            get_subNotes.RemoveAt(index);
        }

        public bool TryRemoveSubNoteAtIndex(int index)
        {
            try
            {
                RemoveSubNoteAtIndex(index);

                return true;
            }
            catch(ArgumentException)
            {
                return false;
            }
        }

        public bool TryRemoveSubNoteWithName(string name)
        {
            Note? subNote = GetSubNoteWithName(name);

            if (subNote == null) return false;
            
            bool output = RemoveSubNote(subNote);

            return output;
        }

        public void RemoveAllSubNotesWhere(Predicate<Note> condition)
        {
            get_subNotes.RemoveAllWhere(condition);
        }

        public void RemoveAllSubNotes()
        {
            get_subNotes.RemoveAll();
        }

        public override string ToString()
        {
            return name + " says " + text;
        }
    }
}