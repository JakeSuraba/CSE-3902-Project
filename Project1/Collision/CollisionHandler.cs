﻿using System;
using System.Xml.Linq;
using Project1.Enemy;
using Project1.Interfaces;
using Project1.PlayerStates;
using System.Reflection;
using System.Collections.Generic;

namespace Project1.Collision
{
    public class CollisionHandler
    {
        XElement responseData;

        private static CollisionHandler instance = new CollisionHandler();

        public static CollisionHandler Instance
        {
            get
            {
                return instance;
            }
        }
        public CollisionHandler()
        {

        }

        public void HandleCollision(ICollidable target, ICollidable source, Direction collisionSide)
        {
            CollisionDebug(target, source, collisionSide); // display debug message to check collision

            Console.WriteLine(Type.GetType("Project1.Player"));
            // retrieve the response corresponding to the two colliding objects
            XElement response = responseData.Element(source.CollisionType).Element(target.CollisionType);

            foreach (XElement command in response.Elements("command"))
            {
                // Get the type of the command object
                Type commandType = Type.GetType(command.Attribute("type").Value);


                List<object> args = new List<object>();

                foreach (XElement arg in command.Elements("arg"))
                {
                    args.Add(parseArg(arg, target, source, collisionSide));
                }

                ConstructorInfo constructor = commandType.GetConstructor(getTypes(args.ToArray()));

                var commandInstance = (ICommand) constructor.Invoke(args.ToArray());

                commandInstance.Execute();
            }

        }

        private object parseArg(XElement arg, ICollidable target, ICollidable source, Direction collisionSide)
        {
            object obj = new object();

            string argTypeName = arg.Attribute("type").Value;
            Type argType = Type.GetType(argTypeName);
            string argValue = arg.Value;

            if (argType.IsPrimitive)
            {
                
                switch (argTypeName)
                {
                    case "System.Int32":
                        obj = int.Parse(argValue);
                        break;
                    case "System.Single":
                        obj = float.Parse(argValue);
                        break;
                    case "System.Boolean":
                        obj = bool.Parse(argValue);
                        break;
                    default:
                        obj = argValue;
                        break;
                }
            }
            else
            {
                switch (argValue)
                {
                    case "SOURCE":
                        obj = source;
                        break;
                    case "TARGET":
                        obj = target;
                        break;
                    case "DIRECTION":
                        obj = collisionSide;
                        break;
                    default:
                        obj = null;
                        Console.WriteLine("unable to parse argument");
                        break;
                }
            }
            return obj;
        }

        Type[] getTypes(object[] objects)
        {
            Type[] types = new Type[objects.Length];
            for(int i = 0; i < types.Length; i++)
            {
                types[i] = objects[i].GetType();
            }
            return types;
        }


        public void LoadResponseData(string path)
        {
            XDocument doc = XDocument.Load(path);
            responseData = doc.Root;
        }

        public void CollisionDebug(ICollidable target, ICollidable source, Direction targetCollisionSide)
        {
            // Might be helpful for handler to determine the Collider type
            CheckTargetType(target);
            CheckSourceType(source);
            // target will always be a mover
            if (source.IsMover)
            {
                Console.WriteLine($"Collision happened between mover and mover, mover get collision from {targetCollisionSide}");
            }
            else
            {
                Console.WriteLine($"Collision happened between mover and non-mover, mover get collision from {targetCollisionSide}");
            }
        }

        private void CheckTargetType(ICollidable obj)
        {
            IEnemy enemy = obj as IEnemy;
            IPlayer link = obj as IPlayer;
            IItem weapon = obj as IItem;
            if (enemy != null)
            {
                Console.WriteLine("Mover is enemy");
            }

            if (link != null)
            {
                Console.WriteLine("Mover is link");
            }

            if (weapon != null)
            {
                Console.WriteLine("Mover is weapon or projectile");
            }
        }

        private void CheckSourceType(ICollidable obj)
        {
            IEnemy enemy = obj as IEnemy;
            IBlock block = obj as IBlock;
            IPlayer link = obj as IPlayer;
            IItem item = obj as IItem;

            if (enemy != null)
            {
                Console.WriteLine("Collider is enemy");
            }

            if (link != null)
            {
                Console.WriteLine("Collider is link");
            }

            if (block != null)
            {
                Console.WriteLine("Collider is block");
            }

            if (item != null)
            {
                Console.WriteLine("Collider is item");
            }
        }

    }
}
