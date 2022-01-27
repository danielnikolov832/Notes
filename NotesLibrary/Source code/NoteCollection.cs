using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace NotesLibrary
{
    // Defines a collection for notes, with the capability to manage said notes
    public class NoteCollection : Collection<Note>
    {
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

        public NoteCollection(IEnumerable<Note> noteEnumerable) : base()
        {
            foreach (Note noteItem in noteEnumerable)
            {
                Add(noteItem);
            }
        }

        #region Collection<Note> overrides

            protected override void InsertItem(int index, Note item)
            {
                bool isItemValid = IsNewItemValid(item);
    
                if (isItemValid)
                {
                    OnBeforeInsertItem?.Invoke(this, item);

                    SubscribeToPropertyChangeEventOfElement(item);
                    
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
                    OnBeforeSetItem?.Invoke(this, item);

                    SubscribeToPropertyChangeEventOfElement(item);

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

                OnBeforeRemoveItem?.Invoke(this, item);

                UnsubscribeToPropertyChangeEventOfElement(item);

                base.RemoveItem(index);
            }
    
            protected override void ClearItems()
            {
                for (int i = 0; i < Count; i++)
                {
                    Note item = this[i];

                    OnBeforeRemoveItem?.Invoke(this, item);

                    UnsubscribeToPropertyChangeEventOfElement(item);
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

        public Note? GetNoteWithName(string name)
        {
            for (int i = 0; i < Count; i++)
            {
                Note noteItem = this[i];

                if (noteItem.getset_name == name) return noteItem;
            }

            return null;
        }

        public bool TryRemoveAt(int index)
        {
            try
            {
                RemoveAt(index);

                return true;
            }
            catch(ArgumentException)
            {
                return false;
            }
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