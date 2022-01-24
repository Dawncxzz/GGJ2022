// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Framework
// {
//     public class InputFilterBase
//     {
//         //MARKER：弃用
//         protected Dictionary<KeyCode, Action> keyAction = null;
//
//         protected InputFilterBase()
//         {
//             keyAction = new Dictionary<KeyCode, Action>();
//         }
//         
//
//         public void AddKeyCodeAction(KeyCode keyCode, Action action)
//         {
//             Action _action = null;
//             if (!keyAction.TryGetValue(keyCode, out _action))
//             {
//                 keyAction.Add(keyCode,action);
//             }
//             else
//             {
//                 _action += action;
//             }
//
//             
//         }
//
//         public virtual void OnUpdate()
//         {
//             foreach (var keyActionPair in this.keyAction)
//             {
//                 if (UnityEngine.Input.GetKey(keyActionPair.Key))
//                 {
//                     keyActionPair.Value.Invoke();
//                 }
//             }
//         }
//         
//
//         public void ClearAction(KeyCode keyCode)
//         {
//             if (this.keyAction.ContainsKey(keyCode))
//             {
//                 keyAction.Remove(keyCode);
//             }
//         }
//         
//     }
// }