#region Using Statements
using System;
using WaveEngine.Common;
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
#endregion

namespace PlatformGameDemoProject
{
    public class MyScene : Scene
    {
        public static string Floor = "Floor";
        public static string Wall = "Wall"; 

        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            PhysicsManager.Physics2DPositionIterations = 15;
            
            var sky = new Entity("Sky")
                .AddComponent(new Sprite("Content/Sky.wpk"))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D()
                {
                    Origin = new Vector2(0.5f, 1),
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight
                });

            var player = CreatePlayer(100, WaveServices.Platform.ScreenHeight - 118);

            EntityManager.Add(CreateFloor("floor2", 250, 400));
            EntityManager.Add(CreateWall("wall1", 0f, 100f));
            EntityManager.Add(CreateWall("wall2", WaveServices.Platform.ScreenWidth, 100f));
            EntityManager.Add(CreateFloor("floor1", WaveServices.Platform.ScreenWidth / 2, WaveServices.Platform.ScreenHeight - 34));
            EntityManager.Add(player);
            EntityManager.Add(sky);

            var anim2D = player.FindChild("TimSprite").FindComponent<Animation2D>();
            anim2D.Play(true);
        }

        private Entity CreateFloor(string name, float x, float y)
        {
             var floor = new Entity(name)
                .AddComponent(new Transform2D()
                {
                    Origin = new Vector2(0.5f, 1f),
                    X = x,
                    Y = y
                })
                .AddComponent(new RectangleCollider())
                .AddComponent(new Sprite("Content/Floor.wpk"))
                .AddComponent(new RigidBody2D() { IsKinematic = true, Mass = 1f, Restitution = 0f, Damping = 0f, Friction = 1f })
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            floor.Tag = Floor;
            
            return floor;
        }

        private Entity CreateWall(string name, float x, float y)
        {
            var wall = new Entity(name)
            .AddComponent(new Transform2D()
            {
                Origin = new Vector2(0.5f, 1),
                X = x,
                Y = y,

            })
            .AddComponent(new RectangleCollider())
            .AddComponent(new Sprite("Content/Floor.wpk"))
            .AddComponent(new RigidBody2D() { IsKinematic = true, Mass = 1f, Rotation = (float)Math.PI / 2.0f })
            .AddComponent(new SpriteRenderer(DefaultLayers.Alpha));
            wall.Tag = Wall;
            return wall;
        }

        private Entity CreatePlayer(float x, float y)
        {
            // Tim sprite entity
            var timSpriteEntity = new Entity("TimSprite")
                .AddComponent(new Transform2D()
                {
                    Origin = new Vector2(0.5f, 0.5f)
                })
                .AddComponent(new Sprite("Content/TimSpriteSheet.wpk"))
                .AddComponent(Animation2D.Create<TexturePackerGenericXml>("Content/TimSpriteSheet.xml")
                    .Add("Idle", new SpriteSheetAnimationSequence() { First = 1, Length = 22, FramesPerSecond = 11 })
                    .Add("Running", new SpriteSheetAnimationSequence() { First = 23, Length = 27, FramesPerSecond = 27 }))
                .AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new TimBehavior());

            //
            // Hay un bug en WaveEngine cuando se intenta agregar un RigidBody con un SpriteSheet. El Wave está tomando el tamaño del spritesheet en vez del tamaño del frame.
            // La solución es agregar el sprite como entity hijo del entity que contiene la física.
            //
            var timPhysicEntity = new Entity("player")
                .AddComponent(new Transform2D()
                {
                    X = x,
                    Y = y,
                    Origin = new Vector2(0.5f, 1),
                    // Aquí definimos el tamaño del entity
                    Rectangle = new RectangleF(0, 0, 100, 100)
                })
                // agregamos un RectangleCollider que toma el tamaño del entity
                .AddComponent(new RectangleCollider())
                // Agregamos el rigid body
                .AddComponent(new RigidBody2D() { IsKinematic = false, Mass = 1f, Friction = 0f, Restitution = 0f })
                // Agregamos el Behavior
                .AddComponent(new PlayerBehavior())
                .AddChild(timSpriteEntity);

            return timPhysicEntity;
        }

    }


}
