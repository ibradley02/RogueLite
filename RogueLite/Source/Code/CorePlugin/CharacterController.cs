﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Duality;
using Duality.Components;
using Duality.Components.Physics;
using Duality.Editor;
using Duality.Plugins.Tilemaps;
using Duality.Plugins.Tilemaps.Properties;

namespace RogueLite
{
    /// <summary>
    /// Applies "retro RPG"-like character movement based on a physical model.
    /// </summary>
    [RequiredComponent(typeof(RigidBody))]
    [EditorHintImage(TilemapsResNames.ImageActorRenderer)]
    public class CharacterController : Component, ICmpUpdatable
    {
        private float speed = 1.0f;
        private float acceleration = 0.2f;
        private Vector2 targetMovement = Vector2.Zero;

        public float Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }
        public float Acceleration
        {
            get { return this.acceleration; }
            set { this.acceleration = value; }
        }
        public Vector2 TargetMovement
        {
            get { return this.targetMovement; }
            set { this.targetMovement = value; }
        }

        void ICmpUpdatable.OnUpdate()
        {
            Transform transform = this.GameObj.Transform;
            RigidBody body = this.GameObj.GetComponent<RigidBody>();

            // Determine how fast we want to be and apply a force to reach the target velocity
            Vector2 clampedTargetMovement = this.targetMovement / MathF.Max(1.0f, this.targetMovement.Length);
            Vector2 targetVelocity = clampedTargetMovement * this.speed;
            Vector2 appliedForce = (targetVelocity - body.LinearVelocity) * body.Mass * this.acceleration;
            body.ApplyLocalForce(appliedForce);
        }
    }
}