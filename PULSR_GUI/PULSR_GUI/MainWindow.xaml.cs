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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PULSR_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int speed = 10; // declaring an integer called speed with value of 10

        bool goUp; // this is the go up boolean
        bool goDown; // this is the go down boolean
        bool goLeft; // this is the go left boolean
        bool goRight; // this is the go right boolean

        public MainWindow()
        {
           
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer(); // adding the timer to the form
            dispatcherTimer.Tick += Timer_Tick; // linking the timer event
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20); // running the timer every 20 milliseconds
            dispatcherTimer.Start(); // starting the timer
            //var rotateAnimation = new DoubleAnimation(7200, 0, TimeSpan.FromSeconds(80));
            //rotatsform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                goDown = true; // down key is pressed go down will be true
            }
            else if (e.Key == Key.Up)
            {
                goUp = true; // up key is pressed go up will be true
            }
            else if (e.Key == Key.Left)
            {
                goLeft = true; // left key is pressed go left will be true
            }
            else if (e.Key == Key.Right)
            {
                goRight = true; // right key is pressed go right will be true
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                goDown = false; // down is released go down will be false
            }
            else if (e.Key == Key.Up)
            {
                goUp = false; // up key is released go up will be false
            }
            else if (e.Key == Key.Left)
            {
                goLeft = false; // left key is released go left will be false
            }
            else if (e.Key == Key.Right)
            {
                goRight = false; // right key is released go right will be false
            }
        }

        private int[] movingPointCoord(int val, int hlen)
        {
            int[] coord = new int[2];
            val *= 6;   //each minute and second make 6 degree
            
            if (val >= 0 && val <= 360)
            {
                coord[0] = 0 + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = 0 - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
          
            return coord;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
         

            controlled_Circle.Content = "Controlled Circle Point :" + "(" + translate1.X + "," + translate1.Y + ")"; // indicator for the controlled circle




            int movingPointTimer = DateTime.Now.Second;
            int[] movingCircleCoord = new int[2]; // moving circle co-ordinate
            movingCircleCoord = movingPointCoord(movingPointTimer, 220); // 

            
            translase.X = movingCircleCoord[0];  // for moving circle X co-ordinate
            translase.Y = movingCircleCoord[1]; //  for moving circle Y co-ordinate

            moving_circle.Content = "Moving Circle Point: " + "(" + translase.X + "," + translase.Y + ")"; // indicator for the moving circle on screen

            if (goUp  && translate1.Y > -270)
            {      
                translate1.Y -= speed;
                // if go up is true and controlled circle co-ordinate is greater then -270
            }
            if (goDown && translate1.Y < 270)
            {
                // if go down is true and and controlled circle co-ordinate is less then 270
                translate1.Y += speed; 
            }
            if (goLeft && translate1.X > -340)
            {
                // if go left is true and controlled circle co-ordinate is greater then -340
             
                translate1.X -= speed;
               
            }
            if (goRight &&  translate1.X < 340 )
            {
                translate1.X += speed;
                // if go right is true and controlled circle co-ordinate is less then 340
            }
        }
    }
}
