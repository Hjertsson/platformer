using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Background : Entity
{

    public Background() : base("background")
    {
        sprite.TextureRect = new IntRect(0, 0, 24, 24);
        sprite.Origin = new Vector2f(12, 12);
    }

    public override void Render(RenderTarget target)
    {
        View view = target.GetView();
        Vector2f topLeft = view.Center - 0.5f * view.Size;
        int tilesX = (int) MathF.Ceiling(view.Size.X / 24);
        int tilesY = (int) MathF.Ceiling(view.Size.Y / 24);
        for (int row = 0; row <= tilesY; row++)
        {
            for (int col = 0; col <= tilesX; col++)
            {
                sprite.TextureRect = row switch //TODO: Skriv tydligare förklaring vad det är som händer här efter föreläsning på onsdag.
                {
                    < 5 => new IntRect(0, 0, 24, 24),
                    5 => new IntRect(24, 0, 24, 24),
                    > 5 => new IntRect(48, 0, 24, 24)
                };
                sprite.Origin = new Vector2f();
                sprite.Position = topLeft + 24 * new Vector2f(col, row);
                target.Draw(sprite);
            }
        }
    }
}