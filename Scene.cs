using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Scene
{
    private readonly Dictionary<string, Texture> textures;
    private readonly List<Entity> entities;

    public Scene()
    {
        textures = new Dictionary<string, Texture>();
        entities = new List<Entity>();
    }
    public void Spawn(Entity entity)
    {
        entities.Add(entity);
        entity.Create(this); // Ett av objekten som ärver från Classen Entity kallar på Create i Class Entity och skapar sig själv.
    }
    public Texture LoadTexture(string name)
    {
        if (textures.TryGetValue(name, out Texture found)) return found; //Läser igenom dictionaryn ifall texturen som har namnet som skickas in, isåfall returnerar den texturen

        string fileName = $"assets/{name}.png";
        Texture texture = new Texture(fileName);
        
        Console.WriteLine(fileName); //TODO: Ett test för att försäkra oss om att en textur bara skapas 1 gång.
        
        textures.Add(name, texture);
        return texture; // Om inte texturen finns så skapas en ny textur med namnet som skickas in i dictionaryn, returnerar sedan den nya texturen.
    }
    public void UpdateAll(float dt)
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            entity.Update(this, dt);
        }

        for (int i = 0; i < entities.Count;)
        {
            Entity entity = entities[i];
            if (entity.Dead) entities.RemoveAt(i);
            else i++;
        }


    }
    public void RenderAll(RenderTarget target)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Render(target);
        }
    }
}