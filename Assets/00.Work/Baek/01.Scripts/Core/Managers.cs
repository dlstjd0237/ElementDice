using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Baek
{
    public class Managers : MonoBehaviour
    {
        private static Managers s_instance;
        private static Managers Instance { get { Init(); return s_instance; } }


        private UIManager _ui = new UIManager();

        public static UIManager UI { get { return Instance?._ui; } }


        private static void Init()
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject { name = "@Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);

                //√ ±‚»≠
                s_instance = go.GetComponent<Managers>();
            }
        }
    }
}

