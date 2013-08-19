using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveEngine.Common;
using WaveEngine.Common.Input;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Framework.UI;
using WaveEngine.Framework.Physics2D;



namespace PlatformGameDemoProject
{
    class PlayerBehavior : Behavior
    {
        [RequiredComponent]
        public Transform2D transform;
        
        [RequiredComponent]
        public RigidBody2D body;
        
        private float speed = 3;
        private float maxSpeed = 3;

        public PlayerBehavior()
            : base("PlayerBehavior")
        {
        }

        protected override void Update(TimeSpan gameTime)
        {
            // Keyboard
            var keyboard = WaveServices.Input.KeyboardState;
            
            if (keyboard.Right == ButtonState.Pressed)
            {
                body.ApplyLinearImpulse(new Vector2(speed, 0));
            }
            else if (keyboard.Left == ButtonState.Pressed)
            {
                body.ApplyLinearImpulse(new Vector2(-speed, 0));
            }
            else
            {
                body.LinearVelocity = new Vector2(0, body.LinearVelocity.Y);
            }
            
            // evita la rotación del rigid body
            body.Rotation = 0f;
            body.AngularVelocity = 0;

            // limita el movimiento a la máxima velocidad
            if (Math.Abs(body.LinearVelocity.X) > maxSpeed)
            {
                body.LinearVelocity = new Vector2(body.LinearVelocity.X > 0? maxSpeed : -maxSpeed, body.LinearVelocity.Y);
            }
        }
    }
}

