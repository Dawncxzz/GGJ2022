using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Logic
{
    public class PropHeal:Prop
    {
        public int CureValue;//治疗值

        public void Cure(Player player)
        {
            player.Cure(CureValue);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            //先获取角色身上对应Type的血条，然后再调用Cure
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Cure");
                Player player = collision.GetComponent<Player>();
                Cure(player);
                PropPool.Recycle(gameObject);
            }
        }

    }
}
