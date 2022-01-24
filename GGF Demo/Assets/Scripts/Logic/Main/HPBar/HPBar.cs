using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Define;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class HPBar : MonoBehaviour
    {
        public HPType HPType;
        public int Health { get => _health; }
        private int _health;
        public int MaxHealth;

        float _scale;

        Slider Slider;

        private void Awake()
        {
            Slider = GetComponent<Slider>();
            if (MaxHealth == 0)
            {
                MaxHealth = 100;
            }
            
            InitListener();
        }

        private void InitListener()
        {
            EventMessageSender.Instance.AddListener(GameDefine.CHANGE_HP_MSG,OnChangeHp); 
        }

        private void OnChangeHp(Message message)
        {
            if (this.HPType.ToString().Equals((string)message["PosTypeStr"]))
            {
                int health = (int) message.Content;
                _health = health;
                Debug.Log($"CurHealth{_health}");
            }
        }


        private void Update()
        {
            _scale = (float)Health / MaxHealth;
            Slider.value = _scale;

        }


        public void Hurt(int num)
        {
            _health -= num;
        }

        public void Cure(int num)
        {
            _health += num;
        }






    }
}
