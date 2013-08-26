using System;

namespace PlatformGameDemo
{
    public class App : WaveEngine.Adapter.Application
    {
        PlatformGameDemoProject.Game game;

        public App()
        {
            this.WindowTitle = "Mobile Gaming Workshop";
            Width = 1280;
            Height = 720;
        }

        public override void Initialize()
        {
            this.WindowTitle = "Mobile Gaming Workshop";
            game = new PlatformGameDemoProject.Game();
            game.Initialize(this.Adapter);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (game != null)
            {
                game.UpdateFrame(elapsedTime);
            }
        }

        public override void Draw(TimeSpan elapsedTime)
        {
            if (game != null)
            {
                game.DrawFrame(elapsedTime);
            }
        }
    }
}

