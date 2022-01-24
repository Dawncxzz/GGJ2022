using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Unity;
using Define;
using Framework;

namespace Pool
{
    public class ObjectPool:MonoBehaviour
    {
        //根据需求重写每个类的对象池
        public GameObject GameObject;
        public int Num;
        protected List<GameObject> UsableGO;//可用
        protected List<GameObject> UsingGO;//正在使用

        private void Awake()
        {
            InitPool();
        }

        public virtual void InitPool()
        {
            UsableGO = new List<GameObject>();
            UsingGO = new List<GameObject>();
            CreatPool(GameObject, Num);
        }

        public void CreatPool(GameObject gameObject,int num)
        {
            for(int i=0;i<num;i++)
            {
                GameObject temp = CreatGO(gameObject);
                Debug.Log(temp.transform.position);
                temp.SetActive(false);
                UsableGO.Add(temp);
            }
        }

        public virtual GameObject Dequeue()
        {
            if(UsableGO.Count<=0)//若没有则实例化一个
            {
                GameObject temp = CreatGO(GameObject);
                UsingGO.Add(temp);
                return temp;
            }
            else
            {
                GameObject temp = UsableGO[0];
                UsableGO.RemoveAt(0);
                UsingGO.Add(temp);
                temp.SetActive(true);
                return temp;
            }
        }

        public virtual void Recycle(GameObject gameObject)
        {
            UsableGO.Add(gameObject);
            UsingGO.Remove(gameObject);
            gameObject.transform.position = this.transform.position;
            gameObject.SetActive(false);
        }

        public virtual void DeleteAll()
        {
            if (UsableGO.Count > 0)
            {
                for (int i = UsableGO.Count - 1;i>=0;i--)
                {
                    Destroy(UsableGO[i]);
                }
                UsableGO.Clear();
            }
            if (UsingGO.Count > 0)
            {
                for (int i = UsingGO.Count - 1; i >= 0; i--)
                {
                    Destroy(UsingGO[i]);
                }
                UsingGO.Clear();
            }
        }


        public virtual GameObject CreatGO(GameObject gameObject)
        {
            GameObject temp= Instantiate(gameObject, this.gameObject.transform);
            return temp;
        }


    }
}
