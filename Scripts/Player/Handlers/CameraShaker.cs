using Godot;
using System;

namespace Player.Handlers
{
    public class CameraShaker : Spatial
    {
        private SceneTreeTween tweening;
        private SceneTreeTween Tweening
        {
            get
            {
                return tweening;
            }
            set
            {
                if (!(tweening is null))
                {
                    if (tweening.IsRunning())
                    {
                        tweening.Kill();
                    }
                }
                tweening = value;
            }
        }

        public override void _Ready()
        {
            // Tweening = CreateTween();
            //Variables.Instance.Jump += () => { if (Variables.Instance.CURRENT_STANDING_STATE == Variables.PlayerStandingState.Standing) MoveToPositionAndBack(new Vector3(0, -.5f, 0), .625f * 2); };
            Variables.Instance.OnFloorChange += (onFloor) =>
            {
                if (onFloor && Variables.Instance.CURRENT_STANDING_STATE == Variables.PlayerStandingState.Standing)
                {
                    MoveToPositionAndBack(new Vector3(0, Mathf.Clamp(Mathf.Sqrt(Mathf.Abs(Variables.Instance.GRAVITY_MOVEMENT.y)), -.9f, 0f), 0), .2f);
                }
            };
        }

        public void MoveToPositionAndBack(Vector3 pos, float time)
        {
            Tweening = CreateTween();
            Tweening.SetProcessMode(Tween.TweenProcessMode.Physics);
            Tweening.TweenProperty(PlayerQuickAccess.CAMERA_SHAKE, "translation", pos, time / 2).SetEase(Tween.EaseType.Out);
            Tweening.Chain().TweenProperty(PlayerQuickAccess.CAMERA_SHAKE, "translation", new Vector3(0, 0, 0), time / 2).SetEase(Tween.EaseType.In);
            Tweening.Play();

        }

        public void Shake(float x, float y, float z, float step = 0.1f, float time = 0.2f, float amount = 1f, int flip = 1)
        {
            Tweening = CreateTween();
            Tweening.SetProcessMode(Tween.TweenProcessMode.Physics);
            bool played = false;

            for (float i = 0; i < amount; i += step)
            {
                PropertyTweener t = Tweening.TweenProperty(PlayerQuickAccess.CAMERA_SHAKE, "translation", new Vector3(Mathf.Clamp(x - i, 0, x) * flip, Mathf.Clamp(y - i, 0, y) * flip, Mathf.Clamp(z - i, 0, z) * flip), time);
                t.SetEase(Tween.EaseType.Out);
                flip *= -1;
                played = true;
            }
            if (played)
                Tweening.Play();
        }
    }

}
