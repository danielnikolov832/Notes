using System.Collections;

namespace ToDoListLibrary
{
    // Defines an encapsulation of a LimitedTimeNoteCollection object 
    // in the end goal of ordered storing of LimitedTimeNote objects
    // to help with their organization
    public class ToDoList : ICollection<LimitedTimeNote>
    {
        private class LimitedTimeNoteComparer : IComparer<LimitedTimeNote>
        {
            public int Compare(LimitedTimeNote? x, LimitedTimeNote? y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return +1;
                }

                return x.get_finalTime.CompareTo(y.get_finalTime);
            }
        }

        private LimitedTimeNoteCollection _limitedTimeNoteCollection;
        public IReadOnlyCollection<LimitedTimeNote> get_noteCollectionAsReadOnly
        {
            get => (IReadOnlyCollection<LimitedTimeNote>)_limitedTimeNoteCollection;
        }

        private LimitedTimeNoteComparer _limitedTimeNoteComparer = new LimitedTimeNoteComparer();


        public LimitedTimeNote this[int index] { get => _limitedTimeNoteCollection[index]; }

        public ToDoList()
        {
            _limitedTimeNoteCollection = new LimitedTimeNoteCollection();
        }

        public ToDoList(List<LimitedTimeNote> noteCollection)
        {
            _limitedTimeNoteCollection = GetSortedNoteCollectionFromList(noteCollection);
        }

        private LimitedTimeNoteCollection GetSortedNoteCollectionFromList(List<LimitedTimeNote> limitedTimeNoteList)
        {
            limitedTimeNoteList.Sort(_limitedTimeNoteComparer.Compare);

            LimitedTimeNoteCollection output = new LimitedTimeNoteCollection();

            output.AddRange(limitedTimeNoteList);

            return output;
        }

        #region ICollection members

            public int Count => _limitedTimeNoteCollection.Count;
    
            public bool IsReadOnly => false;
    
            public void Add(LimitedTimeNote limitedTimeNote)
            {
                for (int i = 0; i < _limitedTimeNoteCollection.Count; i++)
                {
                    LimitedTimeNote limitedTimeNoteItem = _limitedTimeNoteCollection[i];
    
                    bool isNoteSoonerThanItem = _limitedTimeNoteComparer.Compare(limitedTimeNote, limitedTimeNoteItem) < 0;
    
                    if (isNoteSoonerThanItem)
                    {
                        _limitedTimeNoteCollection.Insert(i, limitedTimeNote);
    
                        return;
                    }
                }
    
                _limitedTimeNoteCollection.Add(limitedTimeNote);
            }
    
            public void Clear()
            {
                _limitedTimeNoteCollection.Clear();
            }
    
            public bool Contains(LimitedTimeNote item)
            {
                return _limitedTimeNoteCollection.Contains(item);
            }
    
            public bool Remove(LimitedTimeNote item)
            {
                return _limitedTimeNoteCollection.Remove(item);
            }
    
            public void CopyTo(LimitedTimeNote[] array, int arrayIndex)
            {
                _limitedTimeNoteCollection.CopyTo(array, arrayIndex);
            }
    
            public IEnumerator<LimitedTimeNote> GetEnumerator()
            {
                return _limitedTimeNoteCollection.GetEnumerator();
            }
    
            IEnumerator IEnumerable.GetEnumerator()
            {
                return _limitedTimeNoteCollection.GetEnumerator();
            }
            
        #endregion

        public int IndexOf(LimitedTimeNote item)
        {
            return _limitedTimeNoteCollection.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            _limitedTimeNoteCollection.RemoveAt(index);
        }
    }
}