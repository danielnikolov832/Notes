using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace NotesLibrary
{
    // Defines a collection for notes, with the capability to manage said notes
    public class NoteCollection : Collection<Note>
    {
        // Defines the way in which equality between notes is determined, to ensure no replication of notes
        private class NoteEqualityComparer : IEqualityComparer<Note>
        {
            public bool Equals(Note? x, Note? y)
            {
                if (x == null && y == null) return true;

                if (x == null || y == null) return false;

                bool areWithSameName = x.getset_name == y.getset_name;
                bool areWithSameParent = x.get_parentNote == y.get_parentNote;

                bool output_areEqual = areWithSameName && areWithSameParent;

                return output_areEqual;
            }

            public int GetHashCode([DisallowNullAttribute] Note obj)
            {
                return obj.getset_name.GetHashCode(StringComparison.Ordinal);
            }
        }

        private const string otherNotesEqualToThisOne =
        "There is a note, equal to this one";

        private NoteEqualityComparer _comparer = new NoteEqualityComparer();

        public event EventHandler<Note>? OnBeforeInsertItem;
        public event EventHandler<Note>? OnBeforeSetItem;
        public event EventHandler<Note>? OnBeforeRemoveItem;

        public NoteCollection() : base()
        {

        }

        #region Collection<Note> overrides

            protected override void InsertItem(int index, Note item)
            {
                bool isItemValid = IsNewItemValid(item);
    
                if (isItemValid)
                {
                    SubscribeToPropertyChangeEventOfElement(item);

                    OnBeforeInsertItem?.Invoke(this, item);
                    
                    base.InsertItem(index, item);
                }
                else
                {
                    throw new ArgumentException(otherNotesEqualToThisOne);
                }
            }
    
            protected override void SetItem(int index, Note item)
            {
                bool isItemValid = IsExistingItemValid(index, item);
    
                if (isItemValid)
                {
                    Note previousExistingItem = this[index];

                    UnsubscribeToPropertyChangeEventOfElement(previousExistingItem);
                    SubscribeToPropertyChangeEventOfElement(item);

                    OnBeforeSetItem?.Invoke(this, item);

                    base.SetItem(index, item);
                }
                else
                {
                    throw new ArgumentException(otherNotesEqualToThisOne);
                }
            }
    
            protected override void RemoveItem(int index)
            {
                Note item = this[index];

                UnsubscribeToPropertyChangeEventOfElement(item);

                OnBeforeRemoveItem?.Invoke(this, item);

                base.RemoveItem(index);
            }
    
            protected override void ClearItems()
            {
                for (int i = 0; i < Count; i++)
                {
                    Note item = this[i];

                    UnsubscribeToPropertyChangeEventOfElement(item);

                    OnBeforeRemoveItem?.Invoke(this, item);
                }
    
                base.ClearItems();
            }

        #endregion

        private void SubscribeToPropertyChangeEventOfElement(Note e)
        {
            e.PropertyChanged += Note_PropertyChanged;
        }

        private void UnsubscribeToPropertyChangeEventOfElement(Note e)
        {
            e.PropertyChanged -= Note_PropertyChanged;
        }

        private bool IsNewItemValid(Note noteItem)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_comparer.Equals(this[i], noteItem)) return false;
            }

            return true;
        }

        private bool IsExistingItemValid(int existingItemIndex, Note noteItem)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_comparer.Equals(this[i], noteItem) && (i != existingItemIndex)) return false;
            }

            return true;
        }

        protected virtual void Note_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is Note note)
            {
                bool isItemValid = IsExistingItemValid(IndexOf(note), note);

                if (isItemValid == false)
                {
                    throw new ArgumentException(otherNotesEqualToThisOne);
                }
            }
        }

        public bool TryAdd(Note note)
        {
            try
            {
                Add(note);
            }
            catch(ArgumentException)
            {
                return false;
            }

            return true;
        }
        
        public void AddRange(IEnumerable<Note> noteEnumerable)
        {
            foreach (Note noteItem in noteEnumerable)
            {
                Add(noteItem);
            }
        }

        public bool[] TryAddRange(IEnumerable<Note> noteEnumerable)
        {
            List<bool> output = new List<bool>();

            foreach (Note noteItem in noteEnumerable)
            {
                bool isAdded = TryAdd(noteItem);

                output.Add(isAdded);
            }

            return output.ToArray();
        }

        public Note? GetNoteWithName(string name)
        {
            for (int i = 0; i < Count; i++)
            {
                Note noteItem = this[i];

                if (noteItem.getset_name == name) return noteItem;
            }

            return null;
        }

        public bool TryRemoveNoteWithName(string name)
        {
            Note? noteItemWithName = GetNoteWithName(name);

            if (noteItemWithName == null) return false;
            
            bool output_isRemoved = Remove(noteItemWithName);

            return output_isRemoved;
        }

        public void RemoveAllWhere(Predicate<Note> condition)
        {
            for (int i = 0; i < Count; i++)
            {
                Note noteItem = this[i];

                bool hasCondition = condition.Invoke(noteItem);

                if (hasCondition)
                {
                    RemoveItem(i);
                }
            }
        }

        public void RemoveAll()
        {
            ClearItems();
        }
    }
}