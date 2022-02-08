using System.ComponentModel;

namespace NotesLibrary.Notes
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

        private Note? parentNote = null;
        public Note? get_parentNote
        {
            get => parentNote;
            internal set 
            {
                parentNote = value;
                OnPropertyChanged(nameof(get_parentNote));
            }
        }
                
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Note(string name, string text = "")
        {
            this.name = name;
            this.text = text;
        }

        public Note(string name, Note parentNote, string text = "")
        {
            this.name = name;
            this.text = text;
            this.parentNote = parentNote;
        }
    }
}