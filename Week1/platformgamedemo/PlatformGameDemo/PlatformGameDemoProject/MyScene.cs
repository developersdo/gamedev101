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
#endregion

namespace PlatformGameDemoProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            RenderManager.BackgroundColor = Color.CornflowerBlue;

            var credits = new TextBlock() 
            { 
                Text = "Mobile Developer Group.\nMobile Gaming Workshop.\nProyecto base para salto m�ltiple.\n",
                Margin = new Thickness(10, 10, 0, 0),
                Foreground = Color.Black
            };

            var sky = new Entity("Sky")
                .AddComponent(new Sprite("Content/Sky.wpk"))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D()
                {
                    Origin = new Vector2(0.5f, 1),
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight
                });
            var floor = new Entity("Floor")
                .AddComponent(new Sprite("Content/Floor.wpk"))
                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new Transform2D() 
                { 
                    Origin = new Vector2(0.5f, 1), 
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight 
                });

            // Tim
            var tim = new Entity("Tim")
                .AddComponent(new Transform2D()
                {
                    X = WaveServices.Platform.ScreenWidth / 2,
                    Y = WaveServices.Platform.ScreenHeight - 46,
                    Origin = new Vector2(0.5f, 1)
                })
                .AddComponent(new Sprite("Content/TimSpriteSheet.wpk"))
                .AddComponent(Animation2D.Create<TexturePackerGenericXml>("Content/TimSpriteSheet.xml")
                    .Add("Idle", new SpriteSheetAnimationSequence() { First = 1, Length = 22, FramesPerSecond = 11 })
                    .Add("Running", new SpriteSheetAnimationSequence() { First = 23, Length = 27, FramesPerSecond = 27 }))
                .AddComponent(new AnimatedSpriteRenderer(DefaultLayers.Alpha))
                .AddComponent(new TimBehavior());

            // We add the floor the first so the rocks are on top of Tim
            EntityManager.Add(credits.Entity);
            EntityManager.Add(floor);
            EntityManager.Add(tim);
            EntityManager.Add(sky);

            var anim2D = tim.FindComponent<Animation2D>();
            anim2D.Play(true);
        }
    }
}
