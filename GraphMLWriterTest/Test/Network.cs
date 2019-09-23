﻿using GraphMLWriter.Attributes;

namespace GraphMLWriterTest.Test
{
    public class Network
    {
        #region Public Properties

        public Area[] Areas { get; set; }

        [Edge()]
        public Link[] Links { get; set; }

        [Node()]
        public Point[] Points { get; set; }

        #endregion Public Properties
    }
}