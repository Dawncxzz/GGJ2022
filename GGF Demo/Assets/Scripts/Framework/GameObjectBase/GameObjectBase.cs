using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    public class GameObjectBase:MonoBehaviour
    {
        [HideInInspector]
        public GameObject GameObject;
        [HideInInspector]
        public Transform Transform;
        [HideInInspector]
        public float Speed;
        [HideInInspector]
        public Vector3 Out;//界外坐标

        protected virtual void InitGO()
        {
            GameObject = gameObject;
            Transform = GameObject.transform;
        }


    }
}
