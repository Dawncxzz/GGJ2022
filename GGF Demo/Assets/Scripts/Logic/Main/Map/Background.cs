using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Framework;
using Define;
using UnityEngine.UI;
using System.IO;

namespace Logic
{
    public class Background:MonoSingleton<Background>
    {
        public Material Material;
        public float Speed;
        public int BaseScore;
        public static int OtherScore;
        public int Best;

        public Text Text;

        protected override void Init()
        {
            Material = GetComponent<Renderer>().material;
            //Best = int.Parse(File.ReadAllText("Assets/Scripts/Define/Best.txt"));
            Best = PlayerPrefs.GetInt("score");
            Material.mainTextureOffset = Vector2.zero;
        }

        public void OnUpdate()
        {
            Material.mainTextureOffset += Vector2.right * Speed * 0.1f* Time.deltaTime;
            BaseScore = (int)(Material.mainTextureOffset.x * 100);
            Text.text = (BaseScore + OtherScore).ToString();
        }

    }
}
