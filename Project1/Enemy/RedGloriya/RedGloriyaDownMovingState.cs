﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project1.Animations;
using Project1.Interfaces;

namespace Project1.Enemy
{
    public class RedGloriyaDownMovingState : IEnemyState
    {
        private RedGloriya redGloriya;
        // RedGloriya Down Moving animation data
        private IAnimation downMovingAnimation;
        private ISprite sprite;
        // Down moving state, so Direction.Down
        private Direction currentDirection;
        private Vector2 deltaVector;
        private Random rand = new Random();
        private int choice;
        private int timer;

        public RedGloriyaDownMovingState(RedGloriya redGloriya)
        {
            this.redGloriya = redGloriya;
            downMovingAnimation = new RedGloriyaDownMovingAnimation();
            sprite = SpriteFactory.Instance.CreateAnimatedSprite(downMovingAnimation);
            currentDirection = Direction.Down;
            deltaVector = new Vector2(0, 1);
        }

        public void FireBallAttack()
        {
        }

        // Change current RedGloriya state to RedGloriyaAttackState
        public void BoomerangAttack()
        {
            redGloriya.state = new RedGloriyaAttackState(redGloriya, currentDirection);
        }

        // Change current RedGloriya state to a random direction state.
        public void ChangeDirection()
        {
            choice = rand.Next(1, 4);
            switch (choice)
            {
                case 1: redGloriya.state = new RedGloriyaUpMovingState(redGloriya); break;
                case 2: redGloriya.state = new RedGloriyaLeftMovingState(redGloriya); break;
                case 3: redGloriya.state = new RedGloriyaRightMovingState(redGloriya); break;
            }
        }

        public void Update()
        {
            // When complete an animation cycle length, make a choice
            if (timer++ == downMovingAnimation.CycleLength)
            {
                // 1/4 chance to do a BoomerangAttack
                choice = rand.Next(4);
                if (choice == 0)
                {
                    BoomerangAttack();
                }
                // 3/4 chance to do a ChangeDirection
                else
                {
                    ChangeDirection();
                }
                timer = 0;
            }
            redGloriya.position += deltaVector * redGloriya.movingSpeed;
            sprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, redGloriya.position);
        }
    }
}