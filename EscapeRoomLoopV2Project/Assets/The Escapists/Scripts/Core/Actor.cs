using System;
using UnityEngine;

namespace TheEscapists.Core
{
    [Serializable]
    public class Actor
    {
        public string name;
        public bool isInteractive;

        public Actor(string name, bool isInteractive)
        {
            this.name = name;
            this.isInteractive = isInteractive;
        }
    }
    [Serializable]
    public class Interactor
    {
        public string name;
        public bool isInteractive;

        public Interactor(string name, bool isInteractive)
        {
            this.name = name;
            this.isInteractive = isInteractive;
        }
    }
}