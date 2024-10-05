using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class SoundObj : MonoBehaviour
    {
        public AudioSource AudioSource;
        public bool IsLoop = false;
        public bool IsPlay = true;

        private void Start()
        {
            
            AudioSource.Play();
        }

        private void Update()
        {
            if (!IsLoop && !AudioSource.isPlaying)
            {
                GameObject.Destroy(this.gameObject);// .DestroyObject(this.gameObject);
            }
        }
    }
}
