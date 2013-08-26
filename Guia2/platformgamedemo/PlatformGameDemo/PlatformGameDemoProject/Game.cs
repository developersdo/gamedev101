#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Framework;
using WaveEngine.Framework.Services;
#endregion

namespace PlatformGameDemoProject
{
    public class Game : WaveEngine.Framework.Game
    {
        public override void Initialize(IAdapter adapter)
        {
            base.Initialize(adapter);

            //ViewportManager vm = WaveServices.GetService<ViewportManager>();
            //vm.Activate(800, 600, ViewportManager.StretchMode.Fill);

            ScreenLayers screenLayers = WaveServices.ScreenLayers;
            screenLayers.AddScene<MyScene>();
            screenLayers.Apply();
        }
    }
}
