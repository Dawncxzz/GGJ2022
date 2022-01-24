using Define;
using Framework;

namespace Logic
{
    public class PlayerProperty
    {
        private PosType posType;
        private int m_hp;
        public int MaxHP = 100;

        public PlayerProperty(PosType _posType)
        {
            this.posType = _posType;
            this.HP = MaxHP;
        }

        public int HP
        {
            get
            {
                return m_hp;
            }
            set
            {
                m_hp = value;
                Message message = new Message(GameDefine.CHANGE_HP_MSG, this, m_hp);
                message["PosTypeStr"] = this.posType.ToString();
                EventMessageSender.Instance.SendMessage(message);

                if (value == 0)
                {
                    EventMessageSender.Instance.SendMessage(GameDefine.END_GAME_MSG,this,null);
                }
            }
        }
    }
}