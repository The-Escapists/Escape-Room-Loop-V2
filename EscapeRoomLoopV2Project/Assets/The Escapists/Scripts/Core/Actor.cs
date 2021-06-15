using System;
using UnityEngine;

namespace TheEscapists.Core
{

    [Serializable]
    public class BaseTile
    {
        public string name;
        public int id;
        public Vector2Int position;

        public BaseTile(string _name, Vector2Int _position, int _id)
        {
            name = _name;
            position = _position;
            id = _id;
        }
    }

    [Serializable]
    public class ActorTile : BaseTile
    {
        public bool state;

        public ActorTile(string _name, Vector2Int _position, int _id, bool _state) : base(_name, _position, _id)
        {
            state = _state;
        }
    }

    [Serializable]
    public class InteractorTile : BaseTile
    {
        public bool state;

        public InteractorTile(string _name, Vector2Int _position, int _id, bool _state) : base(_name, _position, _id)
        {
            state = _state;
        }
    }
}