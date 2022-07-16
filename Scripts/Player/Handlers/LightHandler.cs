using Godot;
using System;

namespace Player.Handlers
{
    public class LightHandler : Node
    {
        private Viewport TopView { get; set; }
        private Viewport BottomView { get; set; }
        private Timer Cycle { get; set; }

        public float CurrentLight = 0f;
        public override void _Ready()
        {
            TopView = GetNode<Viewport>("Top");
            BottomView = GetNode<Viewport>("Bottom");
            Cycle = GetNode<Timer>("Capture Light");
            Cycle.Start();

        }
        private void Testing()
        {
            ViewportTexture top = TopView.GetTexture();
            ViewportTexture bottom = BottomView.GetTexture();
            Image topIm = top.GetData();
            Image bottomIm = top.GetData();
            bottomIm.Lock();
            topIm.Lock();
            topIm.ClearMipmaps();
            topIm.SavePng("user://Top Image.png");
        }
        private void UpdateLight()
        {
            ViewportTexture top = TopView.GetTexture();
            ViewportTexture bottom = BottomView.GetTexture();
            Image topIm = top.GetData();
            Image bottomIm = top.GetData();
            bottomIm.Lock();
            topIm.Lock();
            topIm.ClearMipmaps();
            CurrentLight = Help.Math.LightCalculator.GetBrightnessFromTextures(bottomIm, topIm);
            // topIm.SavePng("user://Top Image.png");
            // GD.Print("Updating Light");
            // CurrentLight = Help.Math.LightCalculator.GetBrightnessFromTextures(topIm, bottomIm);
            // Cycle.Start();
            // GD.Print();
            // CallDeferred(nameof(CycledImages), topIm, bottomIm);
        }

        private void CycledImages(Image one, Image two)
        {
            CurrentLight = Help.Math.LightCalculator.GetBrightnessFromTextures(one, two);
            // Cycle.Start();

        }
    }

}
