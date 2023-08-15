using System.Collections.Generic;
using NightmareEchoes.Grid;
using UnityEngine;

//Created by JH
namespace NightmareEchoes.Unit
{
    public class ArcaneBlast : Skill
    {
        public override bool Cast(OverlayTile target, List<OverlayTile> aoeTiles)
        {
            if (target.CheckUnitOnTile())
            {
                var unit = target.CheckUnitOnTile().GetComponent<Units>();
                unit.TakeDamage(damage);
            }

            aoeTiles.Remove(target);
            
            foreach (var tile in aoeTiles)
            {
                if (!tile.CheckUnitOnTile()) continue;
  
                var unit = tile.CheckUnitOnTile().GetComponent<Units>();
                unit.TakeDamage(secondaryDamage);
                
                var direction = tile.transform.position - target.transform.position;
                
                unit.transform.position += direction;
                unit.UpdateLocation();
            }
            
            return true;
        }
    }
}
