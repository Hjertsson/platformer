using System.Text;
using Platformer;
using SFML.Graphics;
using SFML.System;

namespace platformer;

public class Scene
{
    private readonly Dictionary<string, Texture> textures;
    private readonly List<Entity> entities;
    private string nextScene;
    private string currentScene;

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

    public bool TryMove(Entity entity, Vector2f movement)
    {
        entity.Position += movement;
        bool collided = false;

        for (int i = 0; i < entities.Count; i++)
        {
            Entity other = entities[i];
            if(!other.Solid) continue;
            if(other == entity) continue;

            FloatRect boundsA = entity.Bounds;
            FloatRect boundsB = other.Bounds;
            if (Collision.RectangleRectangle(boundsA, boundsB, out Collision.Hit hit))
            {
                entity.Position += hit.Normal * hit.Overlap;
                i = -1;
                collided = true;
            }
        }
        return collided;
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
        HandleSceneChange();
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

    public void Reload()
    {
        nextScene = currentScene;
    }

    public void Load(String scene)
    {
        nextScene = scene;
    }

    private void HandleSceneChange()
    {
        if (nextScene == null) return;
        entities.Clear();
        Spawn(new Background());

        string file = $"assets/{nextScene}.txt";
        Console.WriteLine($"Loading scene '{file}'");

        foreach (string line in File.ReadLines(file,Encoding.UTF8))
        {
            string parsed = line.Trim();
            if (line.Length != 0)
            {
                int commentAt = parsed.IndexOf('#');
                if (commentAt >= 0)
                {
                    parsed = parsed.Substring(0, commentAt);
                    parsed = parsed.Trim();
                }

                string[] words = parsed.Split(" ");
                Vector2f pos = new Vector2f();
                switch (words[0])
                {
                    case "d":
                        Door door = new Door();
                        door.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                        Spawn(door);
                        door.NextRoom = words[3];
                        break;
                    case "k":
                        Key key = new Key();
                        key.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                        Spawn(key);
                        break;
                    case "w":
                        Platform platform = new Platform();
                        platform.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                        Spawn(platform);
                        break;
                    case "h":
                        Hero hero = new Hero();
                        hero.Position = new Vector2f(float.Parse(words[1]), float.Parse(words[2]));
                        Spawn(hero);
                        break;

                }
            }
        }
        
        currentScene = nextScene;
        nextScene = null;
    }

    public bool FindByType<T>(out T found) where T : Entity //TODO: Vad är det som sker i denna funktionen
    {
        foreach (var entity in entities)
        {
            if (!entity.Dead && entity is T typed)
            {
                found = typed;
                return true;
            }
        }
        found = default(T);
        return false;
    }
    
    public void RenderAll(RenderTarget target)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Render(target);
        }
    }
}