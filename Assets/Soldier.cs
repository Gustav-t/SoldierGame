using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public interface ISoldier
    {
        public string Personality { get; }

        void Move();
    }
}
