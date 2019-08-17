using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace OriMod {
  public class OriTile : GlobalTile {
    private int InnerRange => 4;
    private int OuterRange => 13;
    private OriPlayer oPlayer => _localOPlayer ?? (_localOPlayer = Main.LocalPlayer.GetModPlayer<OriPlayer>());
    private OriPlayer _localOPlayer;
    
    private void BurrowEffects(int i, int j, int type, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex, OriPlayer oPlayer) {
      Color orig = drawColor;
      Vector2 playerPos = Main.LocalPlayer.Center.ToTileCoordinates().ToVector2();
      float dist = Vector2.Distance(playerPos, new Vector2(i, j)) - InnerRange;
      dist = Utils.Clamp((OuterRange - dist) / OuterRange, 0, 1);
      if (Abilities.Burrow.CanBurrowAny || oPlayer.burrow.CanBurrow(Main.tile[i, j])) {
        drawColor = Color.Lerp(orig, Color.White, 0.8f * dist);
      }
      else if (Main.tileSolid[type]) {
        drawColor = Color.Lerp(orig, Color.White, 0.3f * dist);
      }
      drawColor.A = orig.A;
    }
    
    public override void DrawEffects(int i, int j, int type, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex) {
      if (oPlayer.burrow.InUse) {
        BurrowEffects(i, j, type, spriteBatch, ref drawColor, ref nextSpecialDrawIndex, oPlayer);
      }
      if (oPlayer.debugMode) {
        Point pos = new Point(i, j);
        if (oPlayer.burrow.BurrowInner.Contains(pos)) {
          drawColor = Color.Red;
        }
        else if (oPlayer.burrow.BurrowEnter.Contains(pos)) {
          drawColor = Color.LimeGreen;
        }
        else if (oPlayer.burrow.BurrowEnterOuter.Contains(pos)) {
          drawColor = Color.Turquoise;
        }
      }
    }
  }
}