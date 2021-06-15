using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using People;
using People.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


namespace Objects.Powers
{
    public class SmokeBomb : PowerTools
    {
        [SerializeField] private GameObject smoke;
        
        public void Start()
        {
            _time = 45;
            TimeBeforeUse = 3;
        }

        protected override bool IsValid() => true;

        protected override void Action()
        {
            var bomb = Instantiate(smoke, transform.position, Quaternion.Euler(-90, 0, 0));
            bomb.GetComponent<SmokeBombManager>().SetPlayer(gameObject);
        }

     

        
    }
}