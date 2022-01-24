using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Logic
{
    public class PropScore:Prop
    {
        public int ScoreValue;

        public void AddScore()
        {
            Background.OtherScore += ScoreValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Cure");
                Player player = collision.GetComponent<Player>();
                AddScore();
                PropPool.Recycle(this.gameObject);
            }
        }




    }
}
