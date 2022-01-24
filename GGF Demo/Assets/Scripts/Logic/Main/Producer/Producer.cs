using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Framework;
using Define;

namespace Logic
{
    public class Producer : MonoSingleton<Producer>
    {

        //产生障碍或者道具

        //产生点
        public GameObject UpHPoint;
        public GameObject UpMPoint;
        public GameObject UpLPoint;
        public GameObject DownLPoint;
        public GameObject DownMPoint;
        public GameObject DownHPoint;

        private bool[] produce = new bool[6];
        private List<Vector3> Points = new List<Vector3>();


        //存放对象池
        public List<GameObject> ObsUp;
        public List<GameObject> ObsDown;

        public List<GameObject> PropUp;
        public List<GameObject> PropDown;

        ObsPool ObsPool1;
        ObsPool ObsPool2;

        PropPool PropPool1;
        PropPool PropPool2;

        Obstacle Obstacle1;
        Obstacle Obstacle2;

        Prop Prop1;
        Prop Prop2;



        public float Duration;//生产时间间隔
        float currentTime1;
        float currentTime2;

        public float ChanceUp;//上面生成道具概率
        public float ChanceDown;//下面生成道具概率

        public float _Speed;
        public static float Speed;//道具行进速度
        public GameObject OutLine;//界外线

        System.Random Ran;

        protected override void Init()
        {
            Speed = _Speed;
            currentTime1 = Duration;
            currentTime2 = Duration;
            Ran = new System.Random();
            Points.Add(UpHPoint.transform.position);
            Points.Add(UpMPoint.transform.position);
            Points.Add(UpLPoint.transform.position);
            Points.Add(DownLPoint.transform.position);
            Points.Add(DownMPoint.transform.position);
            Points.Add(DownHPoint.transform.position);
            for (int i = 0; i < 6; i++)
            {
                produce[i] = false;
            }
        }

        public void OnUpdate()
        {
            if (currentTime1 > 0)
            {
                currentTime1 -= Time.deltaTime;
                if (Obstacle1 != null)
                {
                    if (Obstacle1._endPoint.x > UpHPoint.transform.position.x)
                    {
                        currentTime1 = Duration;
                    }
                    else
                    {
                        for (int a = 0; a < 3; a++)
                        {
                            produce[a] = false;
                        }
                    }
                }
            }
            else
            {
                int i = Ran.Next(ObsUp.Count);

                ObsPool1 = ObsUp[i].GetComponent<ObsPool>();



                Obstacle1 = ObsPool1.Dequeue().GetComponent<Obstacle>();

                if (Obstacle1.ProduceType == ProduceType.random)
                {
                    if (Obstacle1.OccupancyNum > 1)
                    {
                        i = Ran.Next(2);
                    }
                    else
                        i = Ran.Next(3);

                    Obstacle1.transform.position = Points[i];
                }
                else
                {
                    i = (int)(Obstacle1.ProduceType) - 1;
                    Obstacle1.transform.position = Points[i];
                }



                Obstacle1.Speed = Speed;

                Obstacle1.Out = OutLine.transform.position;

                Obstacle1.WakeUp();

                for (int x = Obstacle1.OccupancyNum; x > 0; x--)
                {
                    produce[i] = true;
                    i++;
                }


                int k = Ran.Next(101);
                if (k <= ChanceUp * 100)
                {
                    i = Ran.Next(PropUp.Count);
                    PropPool1 = PropUp[i].GetComponent<PropPool>();
                    k = Ran.Next(3);

                    if (produce[k])
                    {
                        for (int x = 2; x >= 0; x--)
                        {
                            if (!produce[x])
                            {
                                k = x;
                            }
                        }
                    }

                    GameObject gameObject = PropPool1.Dequeue();

                    PropType propType = gameObject.GetComponent<SetType>().PropType;

                    switch(propType)
                    {
                        case PropType.Prop:
                            Prop1 = gameObject.GetComponent<Prop>();
                            break;
                        case PropType.PropHeal:
                            Prop1 = gameObject.GetComponent<PropHeal>();
                            break;
                        case PropType.PropScore:
                            Prop1 = gameObject.GetComponent<PropScore>();
                            break;
                    }


                    Prop1.transform.position = Points[k];
                    Prop1.Speed = Speed;
                    Prop1.Out = OutLine.transform.position;
                    Prop1.WakeUp();
                }

                currentTime1 = Duration;
            }



            if (currentTime2 > 0)
            {
                currentTime2 -= Time.deltaTime;
                if (Obstacle2 != null)
                {
                    if (Obstacle2._endPoint.x > UpHPoint.transform.position.x)
                    {
                        currentTime2 = Duration;
                    }
                    else
                    {
                        for (int a = 3; a < 6; a++)
                        {
                            produce[a] = false;
                        }
                    }
                }
            }
            else
            {
                int j = Ran.Next(ObsDown.Count);
                ObsPool2 = ObsDown[j].GetComponent<ObsPool>();

                Obstacle2 = ObsPool2.Dequeue().GetComponent<Obstacle>();


                if (Obstacle2.ProduceType == ProduceType.random)
                {
                    if (Obstacle1.OccupancyNum > 1)
                    {
                        j = Ran.Next(4, 6);
                    }
                    else
                        j = Ran.Next(3, 6);
                    Obstacle2.transform.position = Points[j];
                }
                else
                {
                    j = 0;
                    switch (Obstacle2.ProduceType)
                    {
                        case ProduceType.hight:
                            j = 5;
                            break;
                        case ProduceType.mid:
                            j = 4;
                            break;
                        case ProduceType.low:
                            j = 3;
                            break;
                    }
                    Obstacle2.transform.position = Points[j];
                }






                Obstacle2.Speed = Speed;
                Obstacle2.Out = OutLine.transform.position;
                Obstacle2.WakeUp();

                for (int x = Obstacle2.OccupancyNum; x > 0; x--)
                {
                    produce[j] = true;
                    j++;
                }

                int l = Ran.Next(101);
                if (l <= ChanceDown * 100)
                {
                    j = Ran.Next(PropDown.Count);
                    PropPool2 = PropDown[j].GetComponent<PropPool>();
                    l = Ran.Next(3, 6);
                    if (produce[l])
                    {
                        for (int y = 3;y < 6;y++)
                        {
                            if(!produce[y])
                            {
                                l = y;
                            }
                        }

                    }


                    GameObject gameObject = PropPool2.Dequeue();

                    PropType propType = gameObject.GetComponent<SetType>().PropType;

                    switch (propType)
                    {
                        case PropType.Prop:
                            Prop2 = gameObject.GetComponent<Prop>();
                            break;
                        case PropType.PropHeal:
                            Prop2 = gameObject.GetComponent<PropHeal>();
                            break;
                        case PropType.PropScore:
                            Prop2 = gameObject.GetComponent<PropScore>();
                            break;
                    }

                    Prop2.transform.position = Points[l];
                    Prop2.Speed = Speed;
                    Prop2.Out = OutLine.transform.position;
                    Prop2.WakeUp();
                }
                currentTime2 = Duration;
            }



        }

        private void OnDrawGizmos()
        {
            foreach (Vector3 vector3 in Points)
            {
                Gizmos.DrawWireSphere(vector3, 0.1f);
            }
        }

    }
}
