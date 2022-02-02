using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace NotesLibrary
{
    // Defines a collection for notes, with the capability to manage said notes
    public class NoteCollection<TNote> : Collection<TNote> where TNote : Note
    {
        // Defines the way in which equality between notes is determined, to ensure no replication of notes
        protected class NoteEqualityComparer : IEqualityComparer<TNote>
        {
            public virtual bool Equals(TNote? x, TNote? y)
            {
                if (x == null && y == null) return true;

                if (x == null || y == null) return false;

                bool areWithSameName = x.getset_name == y.getset_name;
                bool areWithSameParent = x.get_parentNote == y.get_parentNote;

                bool output_areEqual = areWithSameName && areWithSameParent;

                return output_areEqual;
            }

            public int GetHashCode([DisallowNullAttribute] TNote obj)
            {
                return obj.getset_name.GetHashCode(StringComparison.Ordinal);
            }
        }

        private const string otherNotesEqualToThisOne =
        "There is a note, equal to this one";

        protected virtual NoteEqualityComparer none_comparer {get; init;} = new NoteEqualityComparer();

        public event EventHandler<TNote>? OnBeforeInsertItem;
        public event EventHandler<TNote>? OnBeforeSetItem;
        public event EventHandler<TNote>? OnBeforeRemoveItem;

        #region Collection<TNote> overrides
            protected sealed override void InsertItem(int index, TNote item)
            {
                bool isItemValid = IsNewItemValid(item);
    
                if (isItemValid)
                {
                    BindToNoteItem(item);

                    OnBeforeInsertItem?.Invoke(this, item);
                    
                    base.InsertItem(index, item);
                }
                else
                {
                    OnInvalidItem(item);
                }
            }
    
            protected sealed override void SetItem(int index, TNote item)
            {
                bool isItemValid = IsExistingItemValid(index, item);
    
                if (isItemValid)
                {
                    TNote previousExistingItem = this[index];

                    UnbindToNoteItem(previousExistingItem);
                    BindToNoteItem(item);

                    OnBeforeSetItem?.Invoke(this, item);

                    base.SetItem(index, item);
                }
                else
                {
                    OnInvalidItem(item);
                }
            }
    
            protected sealed override void RemoveItem(int index)
            {
                TNote item = this[index];

                UnbindToNoteItem(item);

                OnBeforeRemoveItem?.Invoke(this, item);

                base.RemoveItem(index);
            }
    
            protected sealed override void ClearItems()
            {
                for (int i = 0; i < Count; i++)
                {
                    TNote item = this[i];

                    UnbindToNoteItem(item);

                    OnBeforeRemoveItem?.Invoke(this, item);

                    RemoveItem(i);
                }    
            }

        #endregion

        private void BindToNoteItem(TNote noteItem)
        {
            SubscribeToPropertyChangeEventOfElement(noteItem);

            CustomBind(noteItem);
        }

        // A method, that gives derived classes the ability to perform custom binding
        protected virtual void CustomBind(TNote noteItem)
        {

        }

        private void SubscribeToPropertyChangeEventOfElement(TNote e)
        {
            e.PropertyChanged += Note_PropertyChanged;
        }

        private void UnbindToNoteItem(TNote noteItem)
        {
            UnubscribeToPropertyChangeEventOfElement(noteItem);

            CustomUnbind(noteItem);
        }

        private void UnubscribeToPropertyChangeEventOfElement(TNote e)
        {
            e.PropertyChanged -= Note_PropertyChanged;
        }

        // A method, that gives derived classes the ability to perform custom unbinding
        protected virtual void CustomUnbind(TNote noteItem)
        {

        }

        private bool IsNewItemValid(TNote noteItem)
        {
            for (int i = 0; i < Count; i++)
            {
                if (none_comparer.Equals(this[i], noteItem)) return false;
            }

            return true;
        }

        private bool IsExistingItemValid(int existingItemIndex, TNote noteItem)
        {
            for (int i = 0; i < Count; i++)
            {
                if (none_comparer.Equals(this[i], noteItem) && (i != existingItemIndex)) return false;
            }

            return true;
        }

        // A method that describes what happens when an invalid item is detected
        protected virtual void OnInvalidItem(TNote note)
        {
            throw new ArgumentException(otherNotesEqualToThisOne);
        }

        private void Note_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is TNote note)
            {
                bool isItemValid = IsExistingItemValid(IndexOf(note), note);

                if (isItemValid == false)
                {
                    OnInvalidItem(note);
                }
            }
        }

        public bool TryAdd(TNote note)
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
        
        public void AddRange(IEnumerable<TNote> noteEnumerable)
        {
            foreach (TNote noteItem in noteEnumerable)
            {
                Add(noteItem);
            }
        }

        public bool[] TryAddRange(IEnumerable<TNote> noteEnumerable)
        {
            List<bool> output = new List<bool>();

            foreach (TNote noteItem in noteEnumerable)
            {
                bool isAdded = TryAdd(noteItem);

                output.Add(isAdded);
            }

            return output.ToArray();
        }

        public TNote? GetNoteWithName(string name)
        {
            for (int i = 0; i < Count; i++)
            {
                TNote noteItem = this[i];

                if (noteItem.getset_name == name) return noteItem;
            }

            return null;
        }

        public bool TryRemoveNoteWithName(string name)
        {
            TNote? noteItemWithName = GetNoteWithName(name);

            if (noteItemWithName == null) return false;
            
            bool output_isRemoved = Remove(noteItemWithName);

            return output_isRemoved;
        }

        public void RemoveAllWhere(Predicate<TNote> condition)
        {
            for (int i = 0; i < Count; i++)
            {
                TNote noteItem = this[i];

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