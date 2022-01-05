using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

    public class TaggedObject : UdonSharpBehaviour
    {
        [UdonSynced] [HideInInspector]
        public string EXUR_Tag;
        [UdonSynced] [HideInInspector]
        public int EXUR_LastUsedTime;
    }
