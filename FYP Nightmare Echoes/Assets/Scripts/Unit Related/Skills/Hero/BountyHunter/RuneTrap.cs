using NightmareEchoes.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NightmareEchoes.Unit
{
    public class RuneTrap : Skill
    {
        private bool trapsSet = false;
        
        public override bool Cast()
        {
            
            
            return CheckTraps();
        }

        public override bool Cast(Units unit)
        {
            unit.TakeDamage(damage);
            return true;
        }

        private bool CheckTraps()
        {
            return trapsSet;
        }
    }
}
