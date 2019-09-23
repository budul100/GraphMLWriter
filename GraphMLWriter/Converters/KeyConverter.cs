﻿using GraphMLWriter.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphMLWriter.Converters
{
    internal class KeyConverter
    {
        #region Private Fields

        private int keyIndex;

        #endregion Private Fields

        #region Public Properties

        public IList<keytype> Keys { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public IEnumerable<Func<object, datatype>> GetDataGetters(Type type, keyfortype forType = keyfortype.all)
        {
            var properties = type.GetProperties()
                .Where(p => p.GetCustomAttribute(typeof(Key)) != null).ToArray();

            foreach (var property in properties)
            {
                var name = (property.GetCustomAttribute(typeof(Key)) as Key).Name;

                var key = GetKey(
                    name: name,
                    type: property.PropertyType,
                    forType: forType);

                yield return (input) => GetData(
                    input: input,
                    property: property,
                    key: key);
            }
        }

        public void Initialize()
        {
            Keys = new List<keytype>();
        }

        #endregion Public Methods

        #region Private Methods

        private datatype GetData(object input, PropertyInfo property, keytype key)
        {
            var text = property.GetValue(input).ToString();

            return new datatype
            {
                key = key.id,
                Text = text,
            };
        }

        private keytype GetKey(string name, Type type, keyfortype forType)
        {
            var keyType = GetKeyType(type);

            var key = Keys
                .Where(k => k.attrname == name)
                .Where(k => k.@for == forType)
                .Where(k => k.attrtype == keyType.ToString())
                .SingleOrDefault();

            if (key == null)
            {
                key = new keytype
                {
                    @for = forType,
                    attrname = name,
                    attrtype = keyType.ToString(),
                    id = $"Key-{keyIndex++}",
                };

                Keys.Add(key);
            }

            return key;
        }

        private keytypetype GetKeyType(Type type)
        {
            if (type == typeof(bool))
            {
                return keytypetype.boolean;
            }
            else if (type == typeof(double))
            {
                return keytypetype.@double;
            }
            else if (type == typeof(float))
            {
                return keytypetype.@float;
            }
            else if (type == typeof(int))
            {
                return keytypetype.@int;
            }
            else if (type == typeof(long))
            {
                return keytypetype.@long;
            }
            else
            {
                return keytypetype.@string;
            }
        }

        #endregion Private Methods
    }
}