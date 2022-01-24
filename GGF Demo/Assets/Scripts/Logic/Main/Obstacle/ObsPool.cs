using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using Pool;
using UnityEngine;

namespace Logic
{
    public class ObsPool:ObjectPool
    {
      
        private void Awake()
        {
            InitPool();
        }


        public override GameObject CreatGO(GameObject gameObject)
        {
            GameObject temp= base.CreatGO(gameObject);
            Obstacle obstacle = temp.GetComponent<Obstacle>();
            obstacle.ObsPool = this;
            return temp;
        }

    }
}
