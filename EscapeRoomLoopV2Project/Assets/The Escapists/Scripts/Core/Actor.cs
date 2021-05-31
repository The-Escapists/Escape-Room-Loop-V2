using System;
using UnityEngine;

namespace TheEscapists.Core
{
    [Serializable]
    public struct Actor
    {
        public string name;
        public bool state;
        public Vector2 position;
        public int id;

        public Actor(string _name, bool _state, Vector2 _position, int _id)
        {
            name = _name;
            state = _state;
            position = _position;
            id = _id;
        }
    }

    [Serializable]
    public struct Interactor
    {
        public string name;
        public bool state;
        public Vector2 position;
        public int id;

        public Interactor(string _name, bool _state, Vector2 _position, int _id)
        {
            name = _name;
            state = _state;
            position = _position;
            id = _id;
        }
    }
}