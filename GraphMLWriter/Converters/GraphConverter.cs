﻿using GraphMLWriter.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphMLWriter.Converters
{
    internal class GraphConverter
        : BaseConverter<graphtype>
    {
        #region Private Fields

        private readonly Func<object, IEnumerable<edgetype>> edgesGetter;
        private readonly Func<object, IEnumerable<nodetype>> nodesGetter;

        #endregion Private Fields

        #region Public Constructors

        public GraphConverter(Type type, KeyConverter keyConverter)
            : base(type, keyConverter, keyfortype.graph)
        {
            nodesGetter = GetItemsGetter<nodetype, Node>(
                type: type,
                converterGetter: GetNodeConverterGetter(keyConverter));
            edgesGetter = GetItemsGetter<edgetype, Edge>(
                type: type,
                converterGetter: GetEdgeConverterGetter(keyConverter));
        }

        #endregion Public Constructors

        #region Public Methods

        public override graphtype GetContent(object input)
        {
            var content = new graphtype
            {
                id = idGetter.Invoke(input),
                Items = GetItems(input).ToArray()
            };

            return content;
        }

        #endregion Public Methods

        #region Private Methods

        private IEnumerable<object> GetItems(object input)
        {
            foreach (var dataGetter in dataGetters)
            {
                var data = dataGetter.Invoke(input);

                if (data != null) yield return data;
            }

            var nodes = nodesGetter?.Invoke(input)?
                .Where(n => n != null).ToArray();

            if (nodes?.Any() ?? false)
            {
                foreach (var node in nodes)
                {
                    yield return node;
                }
            }

            var edges = edgesGetter?.Invoke(input)?
                .Where(e => e != null).ToArray();

            if (edges?.Any() ?? false)
            {
                foreach (var edge in edges)
                {
                    yield return edge;
                }
            }
        }

        #endregion Private Methods
    }
}