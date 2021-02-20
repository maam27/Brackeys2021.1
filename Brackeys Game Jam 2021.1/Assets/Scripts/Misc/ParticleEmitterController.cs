using System;
using UnityEngine;

namespace Misc
{
    public class ParticleEmitterController : MonoBehaviour
    {
        public bool isPlaying = false;
        public ParticleSystem system;

        private bool isAlreadyPlaying;

        private void Update()
        {
            if (isPlaying && !isAlreadyPlaying)
            {

                system.Play();
                isAlreadyPlaying = true;
            }
            else
            {
                system.Stop();
                isAlreadyPlaying = false;
            }
                
        }
    }
}