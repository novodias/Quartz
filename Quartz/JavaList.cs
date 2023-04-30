using Newtonsoft.Json;
using System.Collections;

namespace Quartz;

public class JavaList : IList<string>
{
    private readonly IList<string> _list;

    public JavaList()
    {
        _list = new List<string>();
    }

    #region Implementation of IEnumerable

    public IEnumerator<string> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region Implementation of ICollection<T>

    public void Add(string item)
    {
        _list.Add(item);
    }

    public void Clear()
    {
        _list.Clear();
    }

    public bool Contains(string item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public bool Remove(string item)
    {
        return _list.Remove(item);
    }

    public int Count
    {
        get { return _list.Count; }
    }

    public bool IsReadOnly
    {
        get { return _list.IsReadOnly; }
    }

    #endregion

    #region Implementation of IList<T>

    public int IndexOf(string item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, string item)
    {
        _list.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
    }

    public string this[int index]
    {
        get { return _list[index]; }
        set { _list[index] = value; }
    }

    #endregion

    #region Stuff

    public void AddRange(IList<string> items)
    {
        foreach (var java in items)
        {
            Add(java);
        }
    }

    [JsonIgnore]
    private int? _selectedIndex = 0;

    [JsonIgnore]
    public int SelectedIndex
    {
        get 
        {
            return _selectedIndex ?? 0;
        }
        set
        {
            if (value > Count)
            {
                return;
            }

            _selectedIndex = value;
        }
    }

    [JsonIgnore]
    public string SelectedItem
    {
        get
        {
            if (Count == 0)
            {
                return "";
            }

            return this[SelectedIndex];
        }
        set
        {
            if (!Contains(value))
            {
                return;
            }

            SelectedIndex = IndexOf(value);
        }
    }
    #endregion
}
