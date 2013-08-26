using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Components.Animation;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Common.Math;

namespace PlatformGameDemoProject
{
    class TimBehavior : Behavior
    {
        [RequiredComponent]
        public Animation2D anim2D;
        [RequiredComponent]
        public Transform2D trans2D;

        /// <summary>
        /// 1 or -1 indicating right or left respectively
        /// </summary>
        private AnimState currentState, lastState;
        private enum AnimState { Idle, Right, Left };

        public TimBehavior()
            : base("TimBehavior")
        {
            this.anim2D = null;
            this.trans2D = null;
            this.currentState = AnimState.Idle;
        }

        protected override void Update(TimeSpan gameTime)
        {
            currentState = AnimState.Idle;

            // Keyboard
            var keyboard = WaveServices.Input.KeyboardState;
            if (keyboard.Right == ButtonState.Pressed)          // Si presiona flecha derecha
            {
                currentState = AnimState.Right;
            }
            else if (keyboard.Left == ButtonState.Pressed)      // Si presiona flecha izquierda
            {
                currentState = AnimState.Left;
            }

            // Set current animation if that one is diferent
            if (currentState != lastState)
            {
                switch (currentState)
                {
                    case AnimState.Idle:
                        anim2D.CurrentAnimation = "Idle";
                        anim2D.Play(true);
                       // direction = NONE;
                        break;
                    case AnimState.Right:
                        anim2D.CurrentAnimation = "Running";
                        trans2D.Effect = SpriteEffects.None;
                        anim2D.Play(true);
                        //direction = RIGHT;
                        break;
                    case AnimState.Left:
                        anim2D.CurrentAnimation = "Running";
                        trans2D.Effect = SpriteEffects.FlipHorizontally;
                        anim2D.Play(true);
                       // direction = LEFT;
                        break;
                }
            }

            lastState = currentState;

        }

    }
}

