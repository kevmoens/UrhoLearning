using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Urho;
using Urho.Actions;
using Urho.Desktop;
using Urho.Gui;
using Urho.Physics;
using Urho.Shapes;
namespace UrhoLearning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        HelloWorld app;
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (app == null)
            {
                DesktopUrhoInitializer.AssetsDirectory = @"";
                app = await UrhoSurfaceCtrl.Show<HelloWorld>(new Urho.ApplicationOptions(assetsFolder: "Data"));

                //app.PrimarySessionID = PrimarySessionID;

                //Urho.Application.InvokeOnMain(() =>
                //{

                //app.CameraNode.Position = new Vector3(0, 0, 0);
                //app.CameraNode.SetDirection(new Vector3(0, 0, 1));
                //    foreach (var surface in apartments.Surfaces)
                //        app.AddOrUpdateSurface(surface.Value);
                //    foreach (var bulb in apartments.Bulbs)
                //        AddBulb(bulb.Position, bulb.Text, bulb.obj_name, bulb.scale_factor);
                //});
            }
            
        }
    }


    public class HelloWorld : Urho.Application

    {
        Scene scene;
        public Text helloText { get; set; }
        public Node CameraNode { get; set; }
        public Node mainNode { get; set; }
        public Node boxNode { get; set; }
        public Box box { get; set; }
        readonly Urho.Color validPositionColor = Urho.Color.Gray;
        Camera camera;
        public HelloWorld(ApplicationOptions options = null) : base(options) { }

        
        protected override async void Start()

        {
            UI.Root.Resized += Root_Resized;
            var cache = ResourceCache;
            UI.Root.SetColor(new Urho.Color(1f, 1f, 1f));

            scene = new Scene();
            scene.CreateComponent<Octree>();
            scene.CreateComponent<DebugRenderer>();
            CameraNode = scene.CreateChild("Camera");
            camera = CameraNode.CreateComponent<Camera>();

            mainNode = scene.CreateChild("mainNode");
            AddBoxNode();

            //AddPlaneNode();

            AddTextToUI(cache);
                       
            Graphics.WindowTitle = "UrhoSharp Sample";
                       
            // Subscribe to Esc key:
            Input.SubscribeToKeyDown(args => { if (args.Key == Urho.Key.Esc) { Exit(); } });

            SetupViewport();
            
            //AddChicagoModel();
            //All Commented Out is from 0,0,0 to 0,0,1

            //CameraNode.Position = new Vector3(4.0f, 0.0f, 0.0f);
            //CameraNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, -53.13f);

            //From Right to Left
            //CameraNode.Position = new Vector3(4.0f, 0.0f, 3.0f);
            //CameraNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, -90f);

            ////From Left to Right
            //CameraNode.Position = new Vector3(-4.0f, 0.0f, 3.0f);
            //CameraNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 90f);

            ////From Back to Front  0,0,10 to 0,0,0
            //CameraNode.Position = new Vector3(0.0f, 0.0f, 10.0f);
            //CameraNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 180f);



            ////From 3,4,5 
            //CameraNode.Position = new Vector3(-4.0f, 0.0f, 0.0f);
            //CameraNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 53.13f);


            //boxNode.Rotate(new Quaternion(0, 180, 0), TransformSpace.World);
            //await CameraNode.RunActionsAsync(new Urho.Actions.Repeat(new Urho.Actions.MoveBy(1f, new Vector3(0, 1, 0)), 12));


            RotateAnimate(boxNode);
        }

        public static async void RotateAnimate(Node node)
        {
            do
            {
                await node.RunActionsAsync(new RotateBy(2.0f, 0, 360, 0));
            } while (true);
        }

        private void Root_Resized(ResizedEventArgs obj)
        {
            helloText.Position = new IntVector2((obj.Width / 2) - (helloText.Width / 2), (obj.Height / 2) - (helloText.Height / 2));
        }
        private void AddChicagoModel()
        {

            Material bucketMaterial;
            // Model and Physics for the bucket
            Node chicagoNode = mainNode.CreateChild("CHICAGO");
            var bucketModel = chicagoNode.CreateComponent<StaticModel>();           
            bucketMaterial = Material.FromColor(validPositionColor);
            bucketModel.Model = ResourceCache.GetModel("C:/code/Xamarin/Urho/UrhoLearning/UrhoLearning/Data/Models/Object.000.mdl");
            bucketModel.SetMaterial(bucketMaterial);
           
            bucketModel.ViewMask = 0x80000000; //hide from raycasts
            //mainNode.CreateComponent<RigidBody>();
            var shape = chicagoNode.CreateComponent<CollisionShape>();
            shape.SetTriangleMesh(bucketModel.Model, 0, Vector3.One, Vector3.Zero, Quaternion.Identity);
            chicagoNode.SetScale(.05f);
        }
        private void AddPlaneNode()
        {
            Node planeNode = mainNode.CreateChild("planeNode");
            Urho.Shapes.Plane plane = planeNode.CreateComponent<Urho.Shapes.Plane>();
            planeNode.Scale = new Vector3(.5f, .5f, .5f);
            plane.Color = new Urho.Color(1f, 1f, 1f);

            planeNode.Position = new Vector3(0, 0, 9.4999f);
            planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, 270);
            planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 45);
            planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, 45);
        }

        private void AddTextToUI(Urho.Resources.ResourceCache cache)
        {
            helloText = new Text()

            {

                Value = "Hello World from UrhoSharp",

                HorizontalAlignment = Urho.Gui.HorizontalAlignment.Center,

                VerticalAlignment = Urho.Gui.VerticalAlignment.Center

            };

            helloText.SetColor(new Urho.Color(1f, 1f, 1f));

            helloText.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 30);
            helloText.Position = new IntVector2(0, 500);
            UI.Root.AddChild(helloText);
            
        }

        private void AddBoxNode()
        {
            boxNode = mainNode.CreateChild("boxNode");
            box = boxNode.CreateComponent<Box>();
            box.Color = new Urho.Color(0f, 1f, 0f);
            boxNode.Position = new Vector3(0, 0, 3);

            //Translate(Position) just take what it has and moves it more in that direction
            //boxNode.Translate(new Vector3(1, 0, 0));
            //Scale(Size)
            //boxNode.SetScale(.75f);
            //boxNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, 53.13f);



            //if (result != null)
            //{

            //    boxNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, result.Value.Normal.X);
            //    boxNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitY, result.Value.Normal.Y);
            //    boxNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, result.Value.Normal.Z);

            //    //return result.Value.Node;
            //}

        }

        void SetupViewport()
        {
            var renderer = Renderer;
            renderer.SetViewport(0, new Viewport(Context, scene, CameraNode.GetComponent<Camera>(), null));
        }

        protected override void OnUpdate(float timeStep)
        {
            var input = Input;

            const float duration = 1f; //2s
            HandleMovementByKeyPress(input, duration);

            if (input.GetKeyPress(Urho.Key.F))
            {
                //ManualPlacePlane();
                //PlaceDecal();
                PlaceOutletModelParallelToGround();
            }

            base.OnUpdate(timeStep);
        }



        private void PlaceDecal()
        {
            

            Ray cameraRay = camera.GetScreenRay(0.5f, .5f);
            var result = scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry, 0x70000000);
            if (result != null)
            {

                Vector3 hitPos = result.Value.Position;
                Drawable hitDrawable = result.Value.Drawable;


                var targetNode = hitDrawable.Node;
                var decal = targetNode.GetComponent<DecalSet>();

                if (decal == null)
                {
                    var cache = ResourceCache;
                    decal = targetNode.CreateComponent<DecalSet>();
                    var i = ResourceCache.GetImage("Data\\sa_control-panel.png");
                    decal.Material = Material.FromImage(i);
                    decal.Material.CullMode = CullMode.Ccw;
                    decal.Material.ShadowCullMode = CullMode.Ccw;
                    decal.Material.FillMode = FillMode.Solid;
                    decal.Material.DepthBias = new BiasParameters(7685, 0);
                    decal.Material.RenderOrder = 128;

                    //decal.Material = cache.GetMaterial("Materials/UrhoDecal.xml");
                }

                // Add a square decal to the decal set using the geometry of the drawable that was hit, orient it to face the camera,
                // use full texture UV's (0,0) to (1,1). Note that if we create several decals to a large object (such as the ground
                // plane) over a large area using just one DecalSet component, the decals will all be culled as one unit. If that is
                // undesirable, it may be necessary to create more than one DecalSet based on the distance
                decal.AddDecal(hitDrawable, hitPos, camera.Node.Rotation * -1, 0.5f, 1.0f, 1.0f, Vector2.Zero,
                    Vector2.One, 0.0f, 0.1f, uint.MaxValue);
            }
        }


        private void ManualPlacePlane()
        {
            if (box.Color == Urho.Color.Transparent)
            {
                box.Color = Urho.Color.Blue;
            }
            else
            {
                box.Color = Urho.Color.Transparent;
            }

            Ray cameraRay = camera.GetScreenRay(0.5f, .5f);
            var result = scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry, 0x70000000);
            if (result != null)
            {
                //Node planeNode = CameraNode.CreateChild("planeNode");
                //Node planeNode = result.Value.Node.CreateChild("planeNode");

                Node planeNode = mainNode.CreateChild("planeNode");
                //Node planeNode = result.Value.Node.CreateChild("planeNode");
                planeNode.Position = result.Value.Position;
                var pos = new Vector3(result.Value.Position.X - result.Value.Node.Position.X, result.Value.Position.Y - result.Value.Node.Position.Y, result.Value.Position.Z - result.Value.Node.Position.Z);
                var disX = Math.Abs(pos.X);
                var disY = Math.Abs(pos.Y);
                var disZ = Math.Abs(pos.Z);

                //Primary
                if (disX > disY && disX > disZ)
                { //Primary is X  
                    if (pos.X < 0)
                    { //Left
                        planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, 90);
                    }
                    else
                    { //right
                        planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, 270);
                    }
                }
                else if (disY > disZ)
                { //Primary is Y
                    if (pos.Y >= 0)
                    { //top
                        planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, 0);
                    }
                    else
                    { //Bottom
                        planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitZ, 180);
                    }
                }
                else
                { //Primary is Z
                    if (pos.Z < 0)
                    { //Front
                        planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, 270);
                    }
                    else
                    { //Back
                        planeNode.Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, 90);
                    }
                }
                //TODO Next rotate by Node Rotation
                //planeNode.Rotate(Quaternion.FromRotationTo(CameraNode.WorldUp, result.Value.Normal));

                //TODO adjust postion based on drawable vs node
                planeNode.Position = result.Value.Position; //Only set position if we create from Value.Node.  Else if we add node to intersecting node it will use the center of that node
                                                            //planeNode.Translate(new Vector3(0, 0, -1));
                Urho.Shapes.Plane plane = planeNode.CreateComponent<Urho.Shapes.Plane>();
                planeNode.Scale = new Vector3(.5f, .5f, .5f);
                plane.Color = new Urho.Color(1f, 1f, 1f);

                var radian = Vector3.CalculateAngle(cameraRay.Direction, result.Value.Normal);
                var degree = RadianToDegree(radian);
                radian = DegreeToRadian(degree);


                //planeNode.Rotate( Quaternion.FromAxisAngle(Vector3.UnitX, 270)); //Plan typically faces up.  Now flip facing top back toward 0,0,0


                // Create a ball (will be cloned)
                var ballNode = mainNode.CreateChild();
                ballNode.Position = result.Value.Position;
                //ballNode.Rotation = RightCamera.Node.Rotation;
                ballNode.SetScale(0.15f);

                var ball = ballNode.CreateComponent<StaticModel>();
                ball.Model = CoreAssets.Models.Sphere;
                ball.SetMaterial(Material.FromColor(Randoms.NextColor()));
                ball.ViewMask = 0x80000000; //hide from raycasts







            }
        }


        private void PlaceOutletModelParallelToGround()
        {


            Ray cameraRay = camera.GetScreenRay(0.5f, .5f);
            var result = scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry, 0x70000000);
            if (result != null)
            {
                if (result.Value.Node.Name == "")
                {
                    result.Value.Node.Remove();
                }
            }

            result = scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry, 0x70000000);
            if (result != null)
            {
                var outletBase = result.Value.Node.CreateChild("OUTLET");
                //outletBase.Scale = new Vector3(1, 1f, 1) / 10;
                outletBase.Position = result.Value.Position * -0.1f;
                

                var nodeOutlet = outletBase.CreateChild();


                var neg = result.Value.Node.Rotation;
                nodeOutlet.Rotation =  new Quaternion(neg.X, neg.Y, neg.Z, neg.W * -1); //Opposite of the rotation of the Node, if we use result.Value.Position

                


                nodeOutlet.SetScale(1.0f);

                nodeOutlet.CreateComponent<Box>();


               
            }
        }

        private float RadianToDegree(float angle)
        {
            return (float)(angle * (180.0f / Math.PI));
        }
        private float DegreeToRadian(float degree)
        {
            return (float)(Math.PI * degree / 180.0);
        }

        private void HandleMovementByKeyPress(Input input, float duration)
        {
            Urho.Actions.FiniteTimeAction action = null;

            if (input.GetKeyPress(Urho.Key.W))
            {
                action = new MoveBy(duration, new Vector3(0, 0, 1));
            }

            if (input.GetKeyPress(Urho.Key.S))
            {
                action = new MoveBy(duration, new Vector3(0, 0, -1));
            }
            if (input.GetKeyPress(Urho.Key.A))
            {
                action = new MoveBy(duration, new Vector3(-1, 0, 0));
            }
            if (input.GetKeyPress(Urho.Key.D))
            {
                action = new MoveBy(duration, new Vector3(1, 0, 0));
            }
            if (input.GetKeyPress(Urho.Key.Q))
            {
                action = new MoveBy(duration, new Vector3(0, 1, 0));
            }
            if (input.GetKeyPress(Urho.Key.E))
            {
                action = new MoveBy(duration, new Vector3(0, -1, 0));
            }


            if (input.GetKeyPress(Urho.Key.K))
            {
                boxNode.RunActionsAsync(new RotateBy(duration, 0, 10, 0));
            }
            if (input.GetKeyPress(Urho.Key.L))
            {
                boxNode.RunActionsAsync(new RotateBy(duration, 0, -10, 0));
            }
            if (input.GetKeyPress(Urho.Key.J))
            {
                boxNode.RunActionsAsync(new RotateBy(duration, 10, 0, 0));
            }
            if (input.GetKeyPress(Urho.Key.I))
            {
                boxNode.RunActionsAsync(new RotateBy(duration, 10, 0, 0));
            }
            if (input.GetKeyPress(Urho.Key.K))
            {
                boxNode.RunActionsAsync(new RotateBy(duration, -10, 0, 0));
            }
            if (input.GetKeyPress(Urho.Key.O))
            {
                boxNode.RunActionsAsync(new RotateBy(duration, 0, 0, 10));
            }
            if (input.GetKeyPress(Urho.Key.U))
            {
                boxNode.RunActionsAsync(new RotateBy(duration, 0, 0, -10));
            }
            if (input.GetKeyPress(Urho.Key.T))
            {
                if (box.Color == Urho.Color.Transparent)
                {
                    box.Color = Urho.Color.Blue;
                }
                else
                {
                    box.Color = Urho.Color.Transparent;
                }
            }



            if (action != null)
            {
                //can be awaited
                CameraNode.RunActionsAsync(action);
            }
        }
    }
}
