using System.Linq;
using UnityEngine;

namespace Puzzles
{
    public class TVPuzzle : PuzzleMaster
    {


        [Header("Unique Parameters")] 
        [SerializeField] private Antenna antennaA;
        [SerializeField] private Antenna antennaB;
        [Space]
        [SerializeField] private Animator door;
    
        private DetectClick _doorClickInteract;

        protected override void Start()
        {
            base.Start();
            _doorClickInteract = door.GetComponentInChildren<DetectClick>();
        }

        public void CheckIfHasKey()
        {
            var hasKey = false;
            foreach (var item in inventorySystem.idsInInventory.Where(id => id.Contains("LivingRoomDoorKey"))) hasKey = true;
            if (hasKey)
            {
                door.Play("Scene");
                MasterManager.Instance.soundtrackMaster.PlaySoundEffect(3); // Play door open sfx
                inventorySystem.RemoveItem("LivingRoomDoorKey");
                base.FadeInScene();
            }
            else
            {
                MasterManager.Instance.soundtrackMaster.PlaySoundEffect(2); // Play door locked sfx
                MasterManager.Instance.player.helpText.text = "You need something to open this door";
                MasterManager.Instance.player.OpenHelpMenu(true);
            }
        }

        protected override void FadeOutCutscene()
        {
            base.FadeOutCutscene();
            antennaA.GetComponent<Collider>().enabled = false;
            antennaB.GetComponent<Collider>().enabled = false;
            MasterManager.Instance.interactor.DisableLastHitObject(antennaA.gameObject);
            MasterManager.Instance.interactor.DisableLastHitObject(antennaB.gameObject);
        }
        
        public void FocusOnTelevision(bool focus)
        {
            antennaA.canInteract = focus;
            antennaB.canInteract = focus;
            antennaA.GetComponent<DetectClick>().clickEnabled = focus;
            antennaB.GetComponent<DetectClick>().clickEnabled = focus;
            base.FocusOnPuzzleItem(focus);
        }

        protected override void Update()
        {
            if (antennaA.antennaCorrect && antennaB.antennaCorrect && !finished) correct = true;
            if (correct)
            {
                antennaB.masterMix.SetFloat("tv", -80.0f);
                antennaB.masterMix.SetFloat("whiteNoise", -80.0f);
            }
            base.Update();
        }
        
        public void EndLivingRoomCutscene() => base.EndCutScene();
        public void SwitchChildAnimation() => base.BoyAnimations();
        
    }
}