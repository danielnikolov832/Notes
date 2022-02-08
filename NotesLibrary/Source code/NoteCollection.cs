using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using NotesLibrary.Notes;

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

        public event EventHandler<OnActionAndBindStateEventArgs>? OnAfterInsertAndBeforeBindToItem;
        protected virtual void OnAfterInsertAndBeforeBindToItem_Invoke(int index, TNote item)
        {
            OnAfterInsertAndBeforeBindToItem?.Invoke(this, new OnActionAndBindStateEventArgs(index, item));
        }

        public event EventHandler<OnActionAndBindStateEventArgs>? OnAfterSetAndBeforeBindToItem;
        protected virtual void OnAfterSetAndBeforeBindToItem_Invoke(int index, TNote item)
        {
            OnAfterSetAndBeforeBindToItem?.Invoke(this, new OnActionAndBindStateEventArgs(index, item));
        }

        public event EventHandler<OnActionAndBindStateEventArgs>? OnAfterRemoveAndUnbindFromItem;
        protected virtual void OnAfterRemoveAndUnbindToItem_Invoke(int index, TNote item)
        {
            OnAfterRemoveAndUnbindFromItem?.Invoke(this, new OnActionAndBindStateEventArgs(index, item));
        }

        public class OnActionAndBindStateEventArgs : EventArgs
        {
            public int get_index {get; init;}
            public TNote get_noteItem {get; init;}

            public OnActionAndBindStateEventArgs(int index, TNote noteItem)
            {
                get_index = index;
                get_noteItem = noteItem;
            }
        }

        public event EventHandler<OnAfterMoveItemFromOldToNewIndexEventArgs>? OnAfterMoveItemFromOldToNewIndex;
        public class OnAfterMoveItemFromOldToNewIndexEventArgs : EventArgs
        {
            public int get_oldIndex {get; init;}
            public int get_newIndex {get; init;}

            public OnAfterMoveItemFromOldToNewIndexEventArgs(int oldIndex, int newIndex)
            {
                get_oldIndex = oldIndex;
                get_newIndex = newIndex;
            }
        }
        protected virtual void Invoke_OnAfterMoveItemFromOldToNewIndex(int oldIndex, int newIndex)
        {
            OnAfterMoveItemFromOldToNewIndex?.Invoke(this, new OnAfterMoveItemFromOldToNewIndexEventArgs(oldIndex, newIndex));
        }


        #region Collection<TNote> overrides
            protected sealed override void InsertItem(int index, TNote item)
            {
                bool isItemValid = IsNewItemValid(item);
    
                if (isItemValid)
                {
                    base.InsertItem(index, item);

                    OnAfterInsertAndBeforeBindToItem_Invoke(index, item);

                    BindToNoteItem(item);
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

                    base.SetItem(index, item);

                    OnAfterSetAndBeforeBindToItem_Invoke(index, item);

                    BindToNoteItem(item);
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

            base.RemoveItem(index);

            OnAfterRemoveAndUnbindToItem_Invoke(index, item);
        }

        protected sealed override void ClearItems()
            {
                for (int i = 0; i < Count; i++)
                {
                    RemoveItem(i);
                }    
            }

        #endregion

        private void BindToNoteItem(TNote noteItem)
        {
            SubscribeToPropertyChangeEventOfElement(noteItem);

            CustomBindToItem(noteItem);
        }

        // A method, that gives derived classes the ability to perform custom binding
        protected virtual void CustomBindToItem(TNote noteItem)
        {

        }

        private void SubscribeToPropertyChangeEventOfElement(TNote e)
        {
            e.PropertyChanged += Note_PropertyChanged;
        }

        private void UnbindToNoteItem(TNote noteItem)
        {
            UnubscribeToPropertyChangeEventOfElement(noteItem);

            CustomUnbindToItem(noteItem);
        }

        private void UnubscribeToPropertyChangeEventOfElement(TNote e)
        {
            e.PropertyChanged -= Note_PropertyChanged;
        }

        // A method, that gives derived classes the ability to perform custom unbinding
        protected virtual void CustomUnbindToItem(TNote noteItem)
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

        // Gives the ability to move an item
        protected void MoveItemFromOldToNewIndex(int oldIndex, int newIndex)
        {
            TNote noteItemAtOldIndex = this[oldIndex];

            TNote noteItemAtNewIndex = this[newIndex];

            base.SetItem(oldIndex, noteItemAtNewIndex);
            base.SetItem(newIndex, noteItemAtOldIndex);

            Invoke_OnAfterMoveItemFromOldToNewIndex(oldIndex, newIndex);
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

        public bool[] TryAddRange(List<TNote> noteEnumerable)
        {
            bool[] output = new bool[noteEnumerable.Count];

            for (int i = 0; i < noteEnumerable.Count; i++)
            {
                TNote noteItem = noteEnumerable[i];

                bool isAdded = TryAdd(noteItem);

                output[i] = isAdded;
            }

            return output;
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