using System;
using Mogre;

namespace Tutorial
{
    class PlayerController : CharacterController
    {

        protected bool changeGun;

        public bool ChangeGun
        {
            set { changeGun = value; }
            get { return changeGun; }
        }

        protected Character player;

        public Character Player
        {
            set { player = value; }
            get { return player; }
        }
        public PlayerController(Character player)
        {
            speed = 100;
            this.character = player;
        }

        private void MovementsControl(FrameEvent evt)
        {
            Vector3 move = Vector3.ZERO;

            if (forward)
            {
                move += character.Model.Forward;
            }
            if (backward)
            {
                move -= character.Model.Forward;
            }

            if (left)
            {
                move += character.Model.Left;
            }
            if (right)
            {
                move -= character.Model.Left;
            }

            move = move.NormalisedCopy * speed;
            if (accellerate)
            {
                move = move * 2;
            }

            if (move != Vector3.ZERO)
            {
                character.Move(move * evt.timeSinceLastFrame);
            }
        }

        private void MouseControls()
        {
            character.Model.GameNode.Yaw(Mogre.Math.AngleUnitsToRadians(angles.y));
        }

        private void ShootingControls()
        {
            player.Shoot();
            shoot = false;
        }

        public override void Update(FrameEvent evt)
        {
            MovementsControl(evt);
            MouseControls();

            if (shoot)
            {
                ShootingControls();
            }
        }
    }
}
