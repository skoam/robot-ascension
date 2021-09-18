using System;
using System.Collections.Generic;
using UnityEngine;

using Fumiko.Systems.Debug;

namespace Alakajam13th
{
    public class Game : MonoBehaviour
    {
        public static Game instance;

        public int progress = 0;

        private void Update()
        {
        }

        private void Awake()
        {
            if (instance != null)
            {
                LogSystem.instance.DuplicateSingletonError();
            }
            else
            {
                instance = this;
            }
        }
    }
}
