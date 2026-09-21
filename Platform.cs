using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Platform : Entity
{

    public Platform() : base("tileset")
    {
        sprite.TextureRect = new IntRect(0, 0, 18, 18);
        sprite.Origin = new Vector2f(9, 9);
    }

    public override bool Solid => true;
}