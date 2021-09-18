using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alakajam13th
{
    public class EnableChildrenOnProgress : MonoBehaviour
    {
        public int progress = 0;
        public bool disable;

        public Transform[] targets;

        bool done;

        // Start is called before the first frame update
        void Start()
        {
            foreach (Transform target in targets)
            {
                if (disable)
                {
                    target.gameObject.SetActive(true);
                }
                else
                {
                    target.gameObject.SetActive(false);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!done)
            {
                if (Game.instance.progress >= progress)
                {
                    foreach (Transform target in targets)
                    {
                        if (disable)
                        {
                            target.gameObject.SetActive(false);
                        }
                        else
                        {
                            target.gameObject.SetActive(true);
                        }

                        done = true;
                    }
                }
            }
        }
    }
}