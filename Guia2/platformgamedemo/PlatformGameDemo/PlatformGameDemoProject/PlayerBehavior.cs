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
        
        private const int DIR_UP = 1;
        private const int DIR_DOWN = 2;
        private const int DIR_LEFT = 3;
        private const int DIR_RIGHT = 4;

        private float speed = 3;
        private float maxSpeed = 3;
        private bool jumping;
        private bool canJump;

        private float angle = 0;
        private string tag;
        private bool spaceKeyPressed = false;
        private bool doubleJump = false;


        public PlayerBehavior()
            : base("PlayerBehavior")
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            body.OnPhysic2DCollision += body_OnPhysic2DCollision;
        }

        private void body_OnPhysic2DCollision(object sender, Physic2DCollisionEventArgs args)
        {
           // System.Console.WriteLine(args.Body2DA.Owner.Tag + " " + args.Body2DB.Owner.Tag);

            var touchDirection = GetTouchDirection(args.PointA.Value, args.PointB.Value);

            tag = args.Body2DB.Owner.Tag;

            angle = (float)Math.Atan2(args.Normal.X, args.Normal.Y);

            // si no está saltando y toca la pared, sigue en el piso
            if (jumping == false && tag == MyScene.Wall)
                angle = 0;

            // si el contacto fue abajo, entonces está en el piso
            if (touchDirection == DIR_DOWN)
                jumping = false;
            
            // solo puede saltar si toca el piso con la parte de abajo o toca la pared
            if ((tag == MyScene.Floor && touchDirection == DIR_DOWN && jumping == false) ||
                (tag == MyScene.Wall))
            {
                canJump = true;
                doubleJump = false;
            }
            else
            {
                canJump = false;
            }

        }

        private int GetTouchDirection(Vector2 pointA, Vector2 pointB)
        {
            var result = 0;

            if (pointA.Y == pointB.Y)
            {
                if (pointA.Y < transform.Y)
                    result = DIR_UP;
                else
                    result = DIR_DOWN;
            }
            else 
            {
                if (pointA.X < transform.X)
                    result = DIR_LEFT;
                else
                    result = DIR_RIGHT;
            }

            return result;

        }

        protected override void Update(TimeSpan gameTime)
        {
            var impulse = new Vector2();

            // Keyboard
            var keyboard = WaveServices.Input.KeyboardState;
            
            // si no está haciendo el 2do salto, permite el uso de las flechas
            if (doubleJump == false)
            {
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
            }

            // salta cuando se presiona space
            if (keyboard.Space == ButtonState.Pressed && spaceKeyPressed == false)
            {
                spaceKeyPressed = true;

                // puede saltar?
                if (canJump)
                {
                    if (tag == MyScene.Floor) angle = 0;

                    // aplica el impulso
                    impulse.Y = -maxSpeed;
                    impulse.X = 2 * maxSpeed* (float)Math.Sin(angle);
                    body.ApplyLinearImpulse(impulse);
                    
                    jumping = true;
                    canJump = false;

                    // solo si salta desde el suelo el angulo es 0
                    if (angle != 0) doubleJump = true;
                }
            }
            else if (keyboard.Space == ButtonState.Release)
            {
                // cuando suelta la barra espaciadora, deja de saltar
                if (body.LinearVelocity.Y < 0)
                    body.LinearVelocity = new Vector2(body.LinearVelocity.X, 0);

                spaceKeyPressed = false;
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

