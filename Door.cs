using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Door : Entity
{

    public string NextRoom;
    
    public Door() : base("tileset")
    {
        sprite.TextureRect = new IntRect(180, 103, 18, 23);
        sprite.Origin = new Vector2f(9, 11);
    }
    
}