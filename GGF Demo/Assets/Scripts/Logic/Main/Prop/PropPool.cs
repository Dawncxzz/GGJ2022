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
    public class PropPool:ObjectPool
    {
      
        private void Awake()
        {
            InitPool();
        }

        public override GameObject CreatGO(GameObject gameObject)
        {
            GameObject temp = base.CreatGO(gameObject);
            Prop prop = gameObject.GetComponent<Prop>();
            prop.PropPool = this;
            return temp;
        }
    }
}
