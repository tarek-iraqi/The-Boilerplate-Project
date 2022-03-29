using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domain.Common
{
    // https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/
    public class Enumeration : IComparable
    {
        private readonly int _value;
        private readonly string _displayName;

        protected Enumeration() { }

        protected Enumeration(int value, string displayName)
        {
            _value = value;
            _displayName = displayName;
        }

        public int Value => _value;

        public string DisplayName => _displayName;

        public override string ToString() => DisplayName;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();

                if (info.GetValue(instance) is T locatedValue)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => _value.GetHashCode();

        public static T FromValue<T>(int value) where T : Enumeration, new()
            => Parse<T, int>(value, "value", item => item.Value == value);

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
            => Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);

        public int CompareTo(object obj) => Value.CompareTo(((Enumeration)obj).Value);

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }
    }
}
