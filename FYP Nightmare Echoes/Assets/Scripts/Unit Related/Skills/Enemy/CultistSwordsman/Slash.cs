using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NightmareEchoes.Unit
{
    //Written by Ter (stolen from jh)
    public class Slash : Skill
    {
        public override bool Cast(Entity target)
        {
            base.Cast(target);

            StartCoroutine(Attack(target));

            return true;
        }


        IEnumerator Attack(Entity target)
        {
            yield return new WaitForSeconds(0.1f);

            //animation
            animationCoroutine = StartCoroutine(PlaySkillAnimation(thisUnit, "Attacking"));

            yield return new WaitUntil(() => animationCoroutine == null);

            DealDamage(target);
        }
    }
}
