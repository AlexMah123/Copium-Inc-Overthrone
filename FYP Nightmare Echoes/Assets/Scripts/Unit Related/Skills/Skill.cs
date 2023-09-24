using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NightmareEchoes.Grid;
using NightmareEchoes.Unit.Combat;
using UnityEngine;

//Created by JH
namespace NightmareEchoes.Unit
{
    public abstract class Skill : MonoBehaviour
    {
        protected Units thisUnit;
        [SerializeField] protected string skillName;
        [SerializeField] protected int damage;
        [SerializeField] protected int heal;
        [SerializeField] protected int cooldown;
        [SerializeField] protected int range;
        [SerializeField] protected TargetType targetType;
        [SerializeField] protected TargetArea targetArea;
        [SerializeField] protected TargetUnitAlignment targetUnitAlignment;
        
        [SerializeField] protected AOEType aoeType;
        [SerializeField] protected int aoeDuration;
        [SerializeField] protected Color aoeColor;
        
        [SerializeField] protected SkillType skillType;
        
        [SerializeField] protected int secondaryDamage;
        [SerializeField] protected int secondaryRange;

        [Header("Additional Effects")] 
        [SerializeField] protected bool inflictKnockback;
        [SerializeField] protected bool isBackstabbing;
        [SerializeField] protected int backstabBonus = 3;
        [SerializeField] protected bool placable;
        [SerializeField] protected int placableCount;
        [SerializeField] protected GameObject placableGameObject;
        
        [field: TextArea(1,10)][SerializeField] protected string skillDescription;

        #region properties

        public string SkillName
        {
            get => skillName;
            set => skillName = value;
        }

        public int Damage
        {
            get => damage;
            set => damage = value;
        }

        public int Cooldown
        {
            get => cooldown;
            set => cooldown = value;
        }

        public int Range
        {
            get => range;
            set => range = value;
        }

        public TargetType TargetType
        {
            get => targetType;
            set => targetType = value;
        }
        
        public TargetArea TargetArea
        {
            get => targetArea;
            set => targetArea = value;
        }

        public TargetUnitAlignment TargetUnitAlignment
        {
            get => targetUnitAlignment;
            set => targetUnitAlignment = value;
        }

        public AOEType AoeType
        {
            get => aoeType;
            set => aoeType = value;
        }

        public int AoeDuration
        {
            get => aoeDuration;
            set => aoeDuration = value;
        }

        public Color AoeColor
        {
            get => aoeColor;
            set => aoeColor = value;
        }

        public SkillType SkillType
        {
            get => skillType;
            set => skillType = value;
        }

        public int SecondaryDamage
        {
            get => secondaryDamage;
            set => secondaryDamage = value;
        }

        public int SecondaryRange
        {
            get => secondaryRange;
            set => secondaryRange = value;
        }
        
        public string SkillDescription
        {
            get => skillDescription;
            set => skillDescription = value;
        }

        public bool InflictKnockback
        {
            get => inflictKnockback;
            set => inflictKnockback = value;
        }

        public bool Placable
        {
            get => placable;
            set => placable = value;
        }

        public int PlacableCount
        {
            get => placableCount;
            set => placableCount = value;
        }

        public GameObject PlacableGameObject
        {
            get => placableGameObject;
            set => placableGameObject = value;
        }

        #endregion

        private void Awake()
        {
            thisUnit = GetComponent<Units>();
        }

        //Directly on units
        public virtual bool Cast(Units target)
        {
            Vector2 CastFrom = new Vector2(thisUnit.ActiveTile.gridLocation.x, thisUnit.ActiveTile.gridLocation.y);
            Vector2 CastTo = new Vector2(target.ActiveTile.gridLocation.x, target.ActiveTile.gridLocation.y);

            float xDist = CastFrom.x - CastTo.x;
            float yDist = CastFrom.y - CastTo.y;

            if (Mathf.Abs(xDist) > Mathf.Abs(yDist))
            {
                if (xDist < 0)
                {
                    //north
                    thisUnit.Direction = Direction.North;
                    if (target.Direction == Direction.North) 
                    {
                        isBackstabbing = true;
                    }
                }
                else
                {
                    //south
                    thisUnit.Direction = Direction.South;
                    if (target.Direction == Direction.South)
                    {
                        isBackstabbing = true;
                    }
                }
            }
            else
            {
                if (yDist > 0)
                {
                    //east
                    thisUnit.Direction = Direction.East;
                    if (target.Direction == Direction.East)
                    {
                        isBackstabbing = true;
                    }
                }
                else
                {
                    //west
                    thisUnit.Direction = Direction.West;
                    if (target.Direction == Direction.West)
                    {
                        isBackstabbing = true;
                    }
                }
            }

            return false;
        }

        //For ground
        public virtual bool Cast(OverlayTile target, List<OverlayTile> aoeTiles)
        {
            Vector2 CastFrom = new Vector2(thisUnit.ActiveTile.gridLocation.x, thisUnit.ActiveTile.gridLocation.y);
            Vector2 CastTo = new Vector2(target.gridLocation.x, target.gridLocation.y);

            float xDist = CastFrom.x - CastTo.x;
            float yDist = CastFrom.y - CastTo.y;

            if (Mathf.Abs(xDist) > Mathf.Abs(yDist))
            {
                if (xDist < 0)
                {
                    //north
                    thisUnit.Direction = Direction.North;
                    if (target.CheckUnitOnTile()?.GetComponent<Units>().Direction == Direction.North)
                    {
                        isBackstabbing = true;
                    }
                }
                else
                {
                    //south
                    thisUnit.Direction = Direction.South;
                    if (target.CheckUnitOnTile()?.GetComponent<Units>().Direction == Direction.South)
                    {
                        isBackstabbing = true;
                    }
                }
            }
            else
            {
                if (yDist > 0)
                {
                    //east
                    thisUnit.Direction = Direction.East;
                    if (target.CheckUnitOnTile()?.GetComponent<Units>().Direction == Direction.East)
                    {
                        isBackstabbing = true;
                    }
                }
                else
                {
                    //west
                    thisUnit.Direction = Direction.West;
                    if (target.CheckUnitOnTile()?.GetComponent<Units>().Direction == Direction.West)
                    {
                        isBackstabbing = true;
                    }
                }
            }

            return false;
        }
        
        //For specials
        public virtual bool Cast()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool SecondaryCast()
        {
            return false;
        }



        public virtual void Reset()
        {
            
        }
    }

    public enum TargetType
    {
        Single = 0,
        AOE = 1
    }

    public enum TargetArea
    {
        Line = 0,
        Square = 1,
        Crosshair = 2,
        FrontalAttack = 3
    }

    public enum TargetUnitAlignment
    {
        Hostile = 0,
        Friendly = 1,
        Both = 2
    }

    public enum AOEType
    {
        NonAOE = 0,
        Square = 1,
        Cross = 2
    }

    public enum SkillType
    {
        Damage = 0,
        Heal = 1,
        Others = 2
    }
    
}
