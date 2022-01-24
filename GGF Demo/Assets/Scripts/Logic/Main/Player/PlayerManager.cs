using System;
using System.Collections.Generic;
using Define;
using Framework;
using UnityEngine;

namespace Logic
{
    public enum PosType
    {
        Up,
        Down,
    }
    
    public class PlayerManager:Singleton<PlayerManager>
    {
        public static Dictionary<PosType, Player> Pos2Player;
        protected override void Init()
        {
            Pos2Player = new Dictionary<PosType, Player>();
        }

        public T CreatePlayer<T>()where T:Player
        {
            string path = null;
            if (PathDefine.PlayerType2Path.TryGetValue(typeof(T), out path))
            {
                GameObject prefab = Resources.Load<GameObject>(path);
                GameObject player=GameObject.Instantiate(prefab);
                
                Player player_cls = player.GetOrAddComponent<T>();
                Pos2Player.Add(player_cls.PositionType,player_cls);
                
                return player_cls as T;
            }

            return null;
        }

        public void ClearPlayer()
        {
            foreach (var player in Pos2Player.Values)
            {
                player.Dispose();
                GameObject.Destroy(player.GameObject);
            }
            Pos2Player.Clear();
        }

        public void AsyncPlayerPos(PosType posType, Player player,Vector3 offset)
        {
            if (!Pos2Player.ContainsKey(posType))
            {
                throw new Exception($"Cant find {posType} Player!");
            }
            Player player_cls = Pos2Player[posType];
            var position = player_cls.transform.position;
            player.transform.position = new Vector3(position.x,-position.y,0)+offset;
        }

        public void AsyncPlayerState(PosType posType, Player player)
        {
            if (!Pos2Player.ContainsKey(posType))
            {
                throw new Exception($"Cant find {posType} Player!");
            }

            Player player_cls = Pos2Player[posType];

            if (player_cls.Fsm.IsStateChange)
            {
                FSMState<Player> state=player_cls.Fsm.CurrentState;
                player.Fsm.ChangeState(state.GetType());
            }
        }
    }
}