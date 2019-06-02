using Terraria;
using Terraria.GameInput;

namespace OriMod.Abilities {
  public class Glide : Ability {
    internal Glide(OriPlayer oriPlayer, OriAbilities handler) : base(oriPlayer, handler) { }

    private const float MaxFallSpeed = 2f;
    private const float RunSlowdown = 0.125f;
    private const float RunAcceleration = 0.2f;
    private const int StartDuration = 8;
    private const int EndDuration = 10;
    private int CurrTime = 0;

    internal override bool CanUse {
      get {
        return State != States.Ending && !Handler.dash.InUse && !Handler.cDash.InUse && player.velocity.Y > 0;
      }
    }

    protected override void UpdateActive() {
      if (PlayerInput.Triggers.JustPressed.Left || PlayerInput.Triggers.JustPressed.Right) {
        oPlayer.PlayNewSound("Ori/Glide/seinGlideMoveLeftRight" + OriPlayer.RandomChar(5), 0.45f);
      }
      player.maxFallSpeed = MaxFallSpeed;
      player.runSlowdown = RunSlowdown;
      player.runAcceleration = RunAcceleration;
    }
    protected override void UpdateStarting() {
      if (CurrTime == 0) oPlayer.PlayNewSound("Ori/Glide/seinGlideStart" + OriPlayer.RandomChar(3), 0.8f);
      UpdateActive();
    }
    protected override void UpdateEnding() {
      if (CurrTime == 0) oPlayer.PlayNewSound("Ori/Glide/seinGildeEnd" + OriPlayer.RandomChar(3), 0.8f);
      UpdateActive();
    }
    internal override void Tick() {
      if (Handler.dash.InUse) {
        State = States.Inactive;
        CanUse = false;
        CurrTime = 0;
        return;
      }
      if (InUse) {
        if (State == States.Starting) {
          CurrTime++;
          if (CurrTime > StartDuration) {
            State = States.Active;
            CurrTime = 0;
          }
        }
        else if (State == States.Ending) {
          CurrTime++;
          if (CurrTime > EndDuration) {
            State = States.Inactive;
            CanUse = true;
          }
        }
        if (player.velocity.Y < 0 || oPlayer.OnWall || oPlayer.IsGrounded) {
          State = InUse ? States.Ending : States.Inactive;
          CanUse = false;
        }
        
        else if (OriMod.FeatherKey.JustReleased) {
          State = States.Ending;
          CurrTime = 0;
        }
      }
      else {
        if (player.velocity.Y > 0 && !oPlayer.OnWall && (OriMod.FeatherKey.JustPressed || OriMod.FeatherKey.Current)) {
          State = States.Starting;
          CurrTime = 0;
        }
      }
    }
  }
}