using Godot;
using System;

namespace Player.Handlers
{
    public partial class LightHandler : Node
    {
        private SubViewport TopView { get; set; }
        private SubViewport BottomView { get; set; }
        private Timer Cycle { get; set; }

        public float CurrentLight = 0f;
        public override void _Ready()
        {
            TopView = GetNode<SubViewport>("Top");
            BottomView = GetNode<SubViewport>("Bottom");
            Cycle = GetNode<Timer>("Capture Light3D");
            Cycle.Start();

        }
        private void Testing()
        {
            ViewportTexture top = TopView.GetTexture();
            ViewportTexture bottom = BottomView.GetTexture();
            Image topIm = top.GetImage();
            Image bottomIm = top.GetImage();
            topIm.ClearMipmaps();
            topIm.SavePng("user://Top Image.png");
        }
        private void UpdateLight()
        {
            ViewportTexture top = TopView.GetTexture();
            ViewportTexture bottom = BottomView.GetTexture();
            Image topIm = top.GetImage();
            Image bottomIm = top.GetImage();
            topIm.ClearMipmaps();
            CurrentLight = Help.Math.LightCalculator.GetBrightnessFromTextures(bottomIm, topIm);
            // topIm.SavePng("user://Top Image.png");
            // GD.Print("Updating Light3D");
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
