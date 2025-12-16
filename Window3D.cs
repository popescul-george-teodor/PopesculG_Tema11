using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopesculG_Tema11
{
    internal class Window3D : GameWindow
    {

        List<Obj3D> obiecte;
        Obj3D bee,car;
        Camera3D camera = new Camera3D(new Vector3(50, 30, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        PrimitiveType primType = PrimitiveType.Lines;
        KeyboardState previousKeys;
        public Window3D() : base(800, 600, new GraphicsMode(32, 24, 0, 8), "Demo Grafic")
        {
            VSync = VSyncMode.On;
            obiecte = new List<Obj3D>();
            bee = new Obj3D("Bee");
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            Console.WriteLine(locatieFisierSolutie);
            bee.LoadFromObj(locatieFisierSolutie + "\\bee.obj");
            bee.color = Color.Yellow;
            bee.position = new Vector3(3, 2, 0);
            bee.scale = Vector3.One * 0.5f;
            bee.primType = primType;
            car = new Obj3D("Car");
            car.LoadFromObj(locatieFisierSolutie + "\\car.obj");
            car.color = Color.Red;
            car.scale= Vector3.One * 5f;
            car.position = new Vector3(0,1,0);
            car.child = bee;
            car.primType = primType;
            obiecte.Add(car);
            obiecte.Add(new Grid(100, 10));
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.Black); //culoare fundal
            // pentru randare 3d
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            // setari AA (antialiasing)
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.OnResize(e);
            // perspectiva
            GL.Viewport(0, 0, Width, Height);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 256);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
            // camera
            camera.UpdateCamera();
            // Actualizare pozitie obiecte
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            car.position = new Vector3((float)Math.Sin(DateTime.Now.Millisecond/100)*20, 0, 0);
            //bee.position= car.position + new Vector3(0,10,0);
            bee.rotation[1]++;
            KeyboardState currentKeys = Keyboard.GetState();
            // parasire program
            if (currentKeys.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            // miscare camera pe axe
            if (currentKeys.IsKeyDown(Key.Q))
            {
                camera.Move(new Vector3(0, 1, 0));
            }
            if (currentKeys.IsKeyDown(Key.E))
            {
                camera.Move(new Vector3(0, -1, 0));
                camera.UpdateCamera();
            }
            if (currentKeys.IsKeyDown(Key.A))
            {
                camera.Move(new Vector3(0, 0, 1));
                camera.UpdateCamera();
            }
            if (currentKeys.IsKeyDown(Key.D))
            {
                camera.Move(new Vector3(0, 0, -1));
            }
            if (currentKeys.IsKeyDown(Key.W))
            {
                camera.Move(new Vector3(-1, 0, 0));
            }
            if (currentKeys.IsKeyDown(Key.S))
            {
                camera.Move(new Vector3(1, 0, 0));
            }
            if (currentKeys.IsKeyDown(Key.X) && !previousKeys.IsKeyDown(Key.X)) {
                primType++;
                primType = (PrimitiveType)((int)primType % 13);
                car.primType = primType;
                bee.primType = primType;
                Console.WriteLine(primType);
            }
            previousKeys = currentKeys;
            camera.UpdateCamera();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.CornflowerBlue);
            foreach(Obj3D obj in obiecte){
                obj.Draw();
            }
            SwapBuffers();
        }
    }
}
