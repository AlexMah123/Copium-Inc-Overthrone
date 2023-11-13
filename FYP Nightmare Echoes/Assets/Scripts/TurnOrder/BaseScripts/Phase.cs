using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NightmareEchoes.Inputs;
using NightmareEchoes.Unit;
using NightmareEchoes.Unit.Combat;
using NightmareEchoes.Unit.Pathfinding;
using UnityEngine.SceneManagement;
using NightmareEchoes.Grid;


//created by Alex
namespace NightmareEchoes.TurnOrder
{
    public abstract class Phase
    {
        protected TurnOrderController controller;

        public void OnEnterPhase(TurnOrderController turnOrderController)
        {
            //assigns the controller as reference
            controller = turnOrderController;

            //only run once to calculate the turn order and enqueue till the endPhase
            if (!controller.runOnce)
            {
                controller.runOnce = true;
                controller.CalculateTurnOrder();
            }

            //updates the current unit for turn order
            if (controller.CurrentUnitQueue.Count > 0 )
            {
                controller.CurrentUnit = controller.CurrentUnitQueue.Peek();
            }

            #region UI
            if ((controller.currentPhase == controller.playerPhase) && !controller.CurrentUnit.StunToken && !controller.CurrentUnit.IsHostile)
            {
                GameUIManager.Instance.EnableCurrentUI(true);
            }
            else
            {
                GameUIManager.Instance.EnableCurrentUI(false);
            }

            #endregion

            if(controller.currentPhase == controller.playerPhase || controller.currentPhase == controller.enemyPhase)
            {
                CameraControl.Instance.isPanning = false;

                if (controller.CurrentUnit.gameObject != null)
                {
                    CameraControl.Instance.UpdateCameraPan(controller.CurrentUnit.gameObject);
                }
            }

            //updates the UI during each phase & updates status effect 
            GameUIManager.Instance.UpdateTurnOrderUI();
            GameUIManager.Instance.UpdateStatusEffectUI();

            OnEnter();
        }

        public void OnFixedUpdatePhase()
        {
            if (Time.timeScale == 0)
                return;

            if (controller.gameOver)
                return;

            if (controller.cachedHeroesList == null)
            {
                controller.cachedHeroesList = controller.FindAllHeros();
            }

            if (controller.currentPhase != controller.planPhase && controller.currentPhase != controller.startPhase && !controller.gameOver)
            {
                if (controller.FindAllHeros().Count == 0)
                {
                    //Game Over
                    controller.gameOver = true;
                    GeneralUIController.Instance.GameOver();
                }
            }

            if (controller.FindAllEnemies() == null)
            {
                SceneManager.LoadScene(0);
            }

            OnFixedUpdate();
        }

        public void OnUpdatePhase()
        {
            if(GeneralUIController.gameIsPaused)
            {
                return;
            }

            OnUpdate();
        }

        public void OnExitPhase()
        {
            OnExit();

            //disable skill info
            GameUIManager.Instance.EnableSkillInfo(false);

            //reset pathfinding
            PathfindingManager.Instance.isMoving = false;
            PathfindingManager.Instance.hasMoved = false;
            PathfindingManager.Instance.isDragging = false;
            PathfindingManager.Instance.lastAddedTile = null;
            PathfindingManager.Instance.ClearArrow(PathfindingManager.Instance.pathList);
            PathfindingManager.Instance.ClearArrow(PathfindingManager.Instance.tempPathList);

            //clear all rendering 
            RenderOverlayTile.Instance.ClearTargetingRenders();
            CombatManager.Instance.ClearPreviews();
            CombatManager.Instance.turnEnded = false;

            //reset camera panning
            CameraControl.Instance.isPanning = false;

            controller.StopAllCoroutines();
        }

        //overrides
        protected virtual void OnEnter()
        {

        }

        protected virtual void OnFixedUpdate()
        {

        }

        protected virtual void OnUpdate()
        {

        }

        protected virtual void OnExit() 
        { 
            
        }

        
    }
}
