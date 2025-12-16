using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopesculG_Tema11
{
    /// <summary>
    /// Camera 3D isometrica
    /// </summary>
    internal class Camera3D
    {
        private Vector3 target, eye, up_vector;

        public Camera3D(Vector3 eye, Vector3 target, Vector3 up_vector)
        {
            this.eye = eye;
            this.target = target;
            this.up_vector = up_vector;
        }
        /// <summary>
        /// Muta camera cu un delta specificat
        /// </summary>
        /// <param name="delta"></param>
        public void Move(Vector3 delta)
        {
            eye += delta;
            target += delta;
        }
        /// <summary>
        /// Actualizeaza pozitia camerei in scena
        /// </summary>
        public void UpdateCamera()
        {
            Matrix4 lookat = Matrix4.LookAt(eye, target, up_vector);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }
    }
}
