using System;
using System.Collections.Generic;

namespace WpfApp1
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class Pair<TKey, TValue> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected TKey _key;
        protected TValue _value;

        public TKey Key
        {
            get { return _key; }
            set
            {
                if (
                    (_key == null && value != null)
                    || (_key != null && value == null)
                    || !_key.Equals(value))
                {
                    _key = value;
                    NotifyPropertyChanged("Key");
                }
            }
        }

        public TValue Value
        {
            get { return _value; }
            set
            {
                if (
                    (_value == null && value != null)
                    || (_value != null && value == null)
                    || (_value != null && !_value.Equals(value)))
                {
                    _value = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        public Pair()
        {
        }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public Pair(KeyValuePair<TKey, TValue> kv)
        {
            Key = kv.Key;
            Value = kv.Value;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ObservablePairCollection<TKey, TValue> : ObservableCollection<Pair<TKey, TValue>>
    {
        public EventHandler KeyValueChanged;

        public ObservablePairCollection() : base()
        {
        }

        public ObservablePairCollection(IEnumerable<Pair<TKey, TValue>> enumerable)
            : base(enumerable)
        {
        }

        public ObservablePairCollection(List<Pair<TKey, TValue>> list)
            : base(list)
        {
        }

        public ObservablePairCollection(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var kv in dictionary)
            {
                Add(kv.Key, kv.Value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            var pair = new Pair<TKey, TValue>(key, value);
            pair.PropertyChanged += (a, b) => Pair_PropertyChanged(key);
            Add(pair);
        }

        private void Pair_PropertyChanged(TKey key)
        {
            this.KeyValueChanged?.Invoke(this, new PropertyChangedEventArgs(key.ToString()));
        }
    }
}