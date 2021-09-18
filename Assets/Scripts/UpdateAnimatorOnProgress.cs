using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alakajam13th
{
    public class UpdateAnimatorOnProgress : MonoBehaviour
    {
        public Animator animator;
        public string parameter = "progress";

        // Start is called before the first frame update
        void Start()
        {
            animator.SetInteger(parameter, 0);
        }

        float updateStep = 0.5f;
        float currentUpdateStep = 0;
        // Update is called once per frame
        void Update()
        {
            if (!animator)
            {
                return;
            }

            currentUpdateStep += Time.deltaTime;

            if (currentUpdateStep < updateStep)
            {
                return;
            }

            currentUpdateStep = 0;

            animator.SetInteger(parameter, Game.instance.progress);
        }
    }
}