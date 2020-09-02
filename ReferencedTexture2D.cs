using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace OriMod {
  /// <summary>
  /// This class exists simply because I'd rather have direct reference to textures.
  /// instead of getting them from a string dictionary using <see cref="ModContent.GetTexture(string)"/>,
  /// multiple times per frame depending on number of textures.
  /// </summary>
  /// <remarks>
  /// Turns out that even though the description of <see cref="Mod.GetTexture(string)"/> says it's shorthand
  /// for <see cref="ModContent.GetTexture(string)"/>, that is false and the reverse is true
  /// (<see cref="ModContent.GetTexture(string)"/> calls <see cref="Mod.GetTexture(string)"/>)
  /// </remarks>
  public class ReferencedTexture2D {
    public ReferencedTexture2D(string texturePath) {
      if (texturePath is null) {
        throw new System.ArgumentNullException(nameof(texturePath));
      }

      if (OriMod.Instance.TextureExists(texturePath)) {
        texture = OriMod.Instance.GetTexture(texturePath);
      }
      else if (ModContent.TextureExists(texturePath)) {
        texture = ModContent.GetTexture(texturePath);
      }
      else {
        throw new System.ArgumentException($"{texturePath} is not a valid texture path.", nameof(texturePath));
      }
    }

    public readonly Texture2D texture;

    public static implicit operator Texture2D(ReferencedTexture2D ct) => ct.texture;
    public static explicit operator ReferencedTexture2D(Texture2D tx) => new ReferencedTexture2D(tx.Name);
  }
}
