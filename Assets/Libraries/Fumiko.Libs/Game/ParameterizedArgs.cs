using UnityEngine;

namespace Fumiko.Systems
{
    public class ParameterizedArgs
    {
        string[] _strings;
        int[] _ints;
        float[] _floats;
        bool[] _booleans;
        GameObject[] _gameObjects;
        Vector3[] _positions;

        public ParameterizedArgs(int singleInteger)
        {
            _ints = new int[] { singleInteger };
        }

        public ParameterizedArgs(string singleString)
        {
            _strings = new string[] { singleString };
        }

        public ParameterizedArgs(bool singleBool)
        {
            _booleans = new bool[] { singleBool };
        }

        public ParameterizedArgs(Vector3 singleVector)
        {
            _positions = new Vector3[] { singleVector };
        }

        public ParameterizedArgs(GameObject singleGameObject)
        {
            _gameObjects = new GameObject[] { singleGameObject };
        }

        public int FirstInt
        {
            get
            {
                return _ints[0];
            }
        }

        public string FirstString
        {
            get
            {
                return _strings[0];
            }
        }

        public bool IsTrue
        {
            get
            {
                return _booleans[0];
            }
        }

        public Vector3 FirstPosition
        {
            get
            {
                return _positions[0];
            }
        }

        public GameObject FirstGameObject
        {
            get
            {
                return _gameObjects[0];
            }
        }
    }
}