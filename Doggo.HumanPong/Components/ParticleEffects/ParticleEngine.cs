/// http://rbwhitaker.wikidot.com/
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Doggo.HumanPong.Components.ParticleEffects
{
    public class ParticleEngine
    {
        #region Field Region
        private Random random;
        private List<Particle> particles;
        private List<Texture2D> textures;
        #endregion

        #region Property Region
        public Vector2 EmitterLocation { get; set; }
        #endregion

        #region Constructor Region
        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
        }
        #endregion

        #region Method Region
        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 20 + random.Next(40);

            return new Particle(texture, position, Vector2.Zero, angle, angularVelocity, color, size, ttl);
        }

        public void RemoveAllParticles()
        {
            particles.Clear();
        }

        public void Update()
        {
            particles.Add(GenerateNewParticle());

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Matrix scaleMatrix)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, scaleMatrix);
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
        #endregion
    }
}
