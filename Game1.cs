using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Topic_8___Collisions_with_Rectangles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;
        MouseState mouseState;
        
        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;
        Texture2D pacUpTexture;
        Texture2D pacDownTexture;

        Texture2D currentPacTexture; 
        Rectangle pacRect; 

        Texture2D exitTexture;
        Rectangle exitRect, exitRect2;

        Texture2D barrierTexture;
        List<Rectangle> barriers;

        Texture2D coinTexture;
        List<Rectangle> coins;

        List<Rectangle> collisionRects;

        int pacSpeed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            base.Initialize();



            pacSpeed = 3;
            pacRect = new Rectangle(10, 10, 60, 60);
            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(0, 100, 300, 75));
            barriers.Add(new Rectangle(400, 250, 400, 75));
            barriers.Add(new Rectangle(225, 100, 75, 225));
            barriers.Add(new Rectangle(400, 0, 75, 150));
            barriers.Add(new Rectangle(575, 100, 75, 150));
            barriers.Add(new Rectangle(225, 400, 75, 150));
            barriers.Add(new Rectangle(70, 300, 75, 200));


            coins = new List<Rectangle>();

            exitRect = new Rectangle(700, 380, 100, 100);
            exitRect2 = new Rectangle(700, 0, 100, 100);


            collisionRects = new List<Rectangle>();
            collisionRects.Add(exitRect);
            collisionRects.Add(pacRect);
            collisionRects.Add(exitRect2);
            GenerateCoinPosition(20);

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pacDownTexture = Content.Load<Texture2D>("pacDown");
            pacLeftTexture = Content.Load<Texture2D>("pacLeft");
            pacRightTexture = Content.Load<Texture2D>("pacRight");
            pacUpTexture = Content.Load<Texture2D>("pacUp");
            currentPacTexture = Content.Load<Texture2D>("pacRight");
            coinTexture = Content.Load<Texture2D>("coin");
            barrierTexture = Content.Load<Texture2D>("rock_barrier");
            exitTexture = Content.Load<Texture2D>("hobbit_door");
            


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        { 
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;
            }
            foreach (Rectangle barrier in barriers)
                if (pacRect.Intersects(barrier))
                {
                    pacRect.X = barrier.Right;
                }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;
                currentPacTexture = pacRightTexture;
            }
            foreach (Rectangle barrier in barriers)
                if (pacRect.Intersects(barrier))
                {
                    pacRect.X = barrier.Left - pacRect.Width;
                }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacRect.Y -= pacSpeed;
                currentPacTexture = pacUpTexture;
            }
            foreach (Rectangle barrier in barriers)
                if (pacRect.Intersects(barrier))
                {
                    pacRect.Y = barrier.Bottom;
                }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;
            }
            foreach (Rectangle barrier in barriers)
                if (pacRect.Intersects(barrier))
                {
                    pacRect.Y = barrier.Top - pacRect.Height;
                }

            if (pacRect.Right > _graphics.PreferredBackBufferWidth || pacRect.Left < 0)
            {
                if (pacRect.Right > _graphics.PreferredBackBufferWidth)
                {
                    pacRect.X = _graphics.PreferredBackBufferWidth - pacRect.Width;
                }
                else
                {
                    pacRect.X = 0;
                }

            }
            if (pacRect.Bottom > _graphics.PreferredBackBufferHeight || pacRect.Top < 0)
            {
                if (pacRect.Bottom > _graphics.PreferredBackBufferHeight)
                {
                    pacRect.Y = _graphics.PreferredBackBufferHeight - pacRect.Height;
                }
                else
                {
                    pacRect.Y = 0;
                }

            }

            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
                if (exitRect.Contains(mouseState.X, mouseState.Y) || exitRect2.Contains(mouseState.X, mouseState.Y))
                    Exit();

            if (exitRect.Contains(pacRect) || exitRect2.Contains(pacRect))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (Rectangle barrier in barriers)
                _spriteBatch.Draw(barrierTexture, barrier, Color.White);

            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(exitTexture, exitRect2, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);

            DrawCoins(_spriteBatch);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        void GenerateCoinPosition(int numberOfCoins)
        {
            Rectangle coinRect;
            coins = new List<Rectangle>();

            Random random = new Random();
            bool overlap;


            for (int i = 0; i < numberOfCoins; i++)
            {
                overlap = false;
                

                coinRect = new Rectangle(random.Next(0, _graphics.PreferredBackBufferWidth - coinTexture.Width), random.Next(0, _graphics.PreferredBackBufferHeight - coinTexture.Height), coinTexture.Width, coinTexture.Height);
                

                foreach (Rectangle barrier in barriers)
                {
                    if (barrier.Intersects(coinRect))
                    {
                        overlap = true;
                        break;
                    }
                }
                foreach (Rectangle coin in coins)
                {
                    if (coin.Intersects(coinRect))
                    {
                        overlap = true;
                        break;
                    }
                }
                foreach (Rectangle collision in collisionRects)
                {
                    if (collision.Intersects(coinRect))
                    {
                        overlap = true;
                        break;
                    }
                }
               
                if (!overlap)
                {
                    coins.Add(coinRect);
                }
                else
                {
                    i--;
                }
            }     
        }
        void DrawCoins(SpriteBatch spriteBatch)
        {
            foreach (Rectangle coin in coins)
            {
                spriteBatch.Draw(coinTexture, coin, Color.White);
            }
        }


    }
}