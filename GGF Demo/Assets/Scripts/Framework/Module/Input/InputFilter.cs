// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace Framework
// {
//     public class InputFilter<T>:InputFilterBase where T:class
//     {
//         private T m_owner;
//         public InputFilter(T _owner)
//         {
//             m_owner = _owner;
//         }
//         
//         public static InputFilter<T> CreateFilter<T>(T owner) where T : class
//         {
//             InputFilter<T> filter = new InputFilter<T>(owner);
//             return filter;
//         }
//         
//         
//     }
// }