using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Alex
namespace NightmareEchoes.Unit.Enemy
{
    public class MeleeEnemy : BaseUnit
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }



        #region Abilities()
        public override void BasicAttack()
        {
            Direction = Direction.South;
        }

        public override void Passive()
        {

        }

        public override void Skill1()
        {

        }

        public override void Skill2()
        {

        }

        public override void Skill3()
        {

        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
        }

        [ContextMenu("Take Damage (5)")]
        public void TestDamage()
        {
            TakeDamage(2);
        }

        #endregion
    }
}
