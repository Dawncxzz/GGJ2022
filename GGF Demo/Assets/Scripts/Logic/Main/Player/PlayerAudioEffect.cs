using UnityEngine;

namespace Logic
{
    public class PlayerAudioEffect:MonoBehaviour
    {
        public AK.Wwise.Event player_jump_event;
        public AK.Wwise.Event player_doublejump_event;
        public AK.Wwise.Event player_dash_event;
        public AK.Wwise.Event player_slide_event;
        public AK.Wwise.Event player_dive_event;
        public AK.Wwise.Event player_hurt_event;
        public AK.Wwise.Event player_heal_event;
        


        public void OnJumpEvent()
        {
            player_jump_event.Post(gameObject);
        }

        public void OnDoubleJumpEvent()
        {
            player_doublejump_event.Post(gameObject);
        }

        public void OnDashEvent()
        {
            player_dash_event.Post(gameObject);
        }

        public void OnSlideEvent()
        {
            player_slide_event.Post(gameObject);
        }

        public void OnDiveEvent()
        {
            player_dive_event.Post(gameObject);
        }

        public void OnHurtEvent()
        {
            player_hurt_event.Post(gameObject);
        }

        public void OnHealEvent()
        {
            player_heal_event.Post(gameObject);
        }
    }
}