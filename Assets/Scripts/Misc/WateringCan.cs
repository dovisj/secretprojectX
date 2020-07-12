using System;
using UnityEngine;

namespace Misc
{
    public class WateringCan : MonoBehaviour
    {
        public float _waterAmount = 100f;
        private ParticleSystem _particleSystem;

        public float WaterAmount
        {
            get => _waterAmount;
            set
            {
                _waterAmount = value;
                if (_waterAmount < 50f)
                {
                    StopParticles();
                }
            }
        }

        private void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _particleSystem.Stop();
        }

        public void RunParticles()
        {
            _particleSystem.Play();
        }
        
        public void StopParticles()
        {
            _particleSystem.Stop();
        }
    }
}
