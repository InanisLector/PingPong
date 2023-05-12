using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Gameobject
{
    public abstract class GameObject
    {
        private int id;

        public Vector2f position;
        private Vector2f pivot;
        private Shape _sprite;

        public Shape Sprite
        {
            get
            {
                _sprite.Position = position + pivot;
                return _sprite;
            }
            protected set
                => _sprite = value;
        }

        public GameObject(Shape sprite)
        {
            _sprite = sprite;

            pivot.X = sprite.Scale.X / 2;
            pivot.Y = sprite.Scale.Y / 2;

            GameObjectLoop.GetInstance().AddObject(this);

            Awake();
        }
        
        public abstract void Awake();
        public abstract void Update();
        protected void Destroy()
        {
            GameObjectLoop.GetInstance().RemoveObject(id);
        }
    }

    internal class GameObjectLoop
    {
        private static GameObjectLoop? instance;

        private int numOfObjects;
        private Dictionary<int, GameObject> objects;

        private RenderWindow window;

        private GameObjectLoop()
        {
            numOfObjects = 0;

            window = new(new VideoMode(1600, 900), "Ping pong");
        }

        public static GameObjectLoop GetInstance()
        {
            if(instance == null)
                instance = new GameObjectLoop();

            return instance;
        }


        public void InvokeUpdate()
        {
            foreach (var obj in objects.Values)
            {
                obj.Update();
            }
        }

        public void InvokeRender()
        {
            window.Clear(Color.Black);

            foreach (GameObject obj in objects.Values)
            {
                window.Draw(obj.Sprite);
            }

            window.Display();
        }

        public void AddObject(GameObject obj)
        {
            objects.Add(numOfObjects++, obj);
        }

        public void RemoveObject(int id)
        {
            objects.Remove(id);
        }
    }
}
