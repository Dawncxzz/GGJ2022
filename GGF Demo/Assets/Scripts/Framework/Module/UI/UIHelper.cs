using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework
{
    public static class UIHelper
    {
        public static void AddListener(this Button button,UnityAction clickEventHandler )
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(clickEventHandler);
        }
    }
}