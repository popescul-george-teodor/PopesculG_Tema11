using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;


namespace PopesculG_Tema11
{
    /// <summary>
    /// Clasa abstracta pentru un obiect 3D de baza
    /// </summary>
    class Obj3D
    {
        /// <summary>
        /// pozitia, rotatia si marimea obiectului 3D
        /// </summary>
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        public List<Vector3> vertex { get; set; }
        public Color color { get; set; }
        public bool visible { get; set; }

        public String name;

        public PrimitiveType primType;

        public Obj3D child;

        public Obj3D(String name="none")
        {
            position = new Vector3(0, 0, 0);
            rotation = new Vector3(0, 0, 0);
            scale = new Vector3(1, 1, 1);
            color = Color.White;
            primType = PrimitiveType.LineLoop;
            visible = true;
            this.name = name;
        }

        public void LoadFromObj(string filePath)
        {
            vertex = new List<Vector3>();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (line.StartsWith("v "))
                {
                    string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[2]);
                    float z = float.Parse(parts[3]);
                    vertex.Add(new Vector3(x, y, z));
                }
            }
        }

        /// <summary>
        /// Functie abstracta pentru desenarea obiectului 3D
        /// </summary>
        public virtual void Draw()
        {
            if (visible)
            {
                GL.PushMatrix();
                GL.Translate(position);
                GL.Rotate(rotation.X, Vector3.UnitX);
                GL.Rotate(rotation.Y, Vector3.UnitY);
                GL.Rotate(rotation.Z, Vector3.UnitZ);
                GL.Scale(scale);
                GL.Color4(color);
                GL.Begin(primType);
                foreach (Vector3 v in vertex)
                {
                    GL.Vertex3(v);
                }
                GL.End();
                if (child != null) child.Draw();
                GL.PopMatrix();
            }
        }
    }
    
    class Grid : Obj3D
    {
        private float size;
        private int divisions;
        private Color DEFAULT_COLOR = Color.Aqua;
        public Grid(float size, int divisions)
        {
            this.size = size;
            this.divisions = divisions;
            this.visible = true;
            this.color = DEFAULT_COLOR;
        }
        public override void Draw()
        {
            if (visible)
            {
                GL.Color3(color);
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.Lines);
                float step = size / divisions;
                for (int i = -divisions; i <= divisions; i++)
                {
                    GL.Vertex3(i * step, 0, -size);
                    GL.Vertex3(i * step, 0, size);
                    GL.Vertex3(-size, 0, i * step);
                    GL.Vertex3(size, 0, i * step);
                }
                GL.End();
            }
        }
    
    }
}
