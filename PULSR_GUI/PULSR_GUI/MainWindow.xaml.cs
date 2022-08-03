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
        double screen_width = System.Windows.SystemParameters.PrimaryScreenWidth;
        double screen_height = System.Windows.SystemParameters.PrimaryScreenHeight;
        double gui_width = 0;
        double gui_height = 0;
        int effector_x = 0; //to be gotten from forward kinematics
        int effector_y = 0; //to be gotten from forward kinematics

        int speed = 10; //value of 10, defines the speed with which the controlled coordinates move, used for design test only

        bool goUp; // this is the go up boolean
        bool goDown; // this is the go down boolean
        bool goLeft; // this is the go left boolean
        bool goRight; // this is the go right boolean

        public MainWindow()
        {  
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer(); // adding the timer to the form, what is the timer for?
            dispatcherTimer.Tick += Timer_Tick; // linking the timer event
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20); // running the timer every 20 milliseconds
            dispatcherTimer.Start(); // starting the timer
            //var rotateAnimation = new DoubleAnimation(7200, 0, TimeSpan.FromSeconds(80));
            //rotatsform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            /*
             * comment the function of input arguments and
             * function of function itslef
             */
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
            /*
             * comment the function of input arguments and
             * function of function itslef
             */
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
            //make hlen argument type double as the vawriable it is to take is double and typecasting to int can cause data loss
            /*
             * define what this function does by defining
             * 1. role of input arguments
             * 2. role of returned integer array
             */
            int[] coord = new int[2]; //good use of dynamic memory, thumbs up
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
            /*
             * comment the function of input arguments and
             * function of function itslef
             */

            gui_width = main_window_grid.ActualWidth;
            gui_height = main_window_grid.ActualHeight;

            double path_diameter = gui_height*0.7;
            t_mode_circular_path.Width = path_diameter;
            t_mode_circular_path.Height = path_diameter;

            //controlled circle coordinate label indicator, to be removed later on
            controlled_Circle.Content = "Controlled Circle Point :" + "(" + effector_coord_xy_translator.X + "," + effector_coord_xy_translator.Y + ")"; // indicator for the controlled circle


            int movingPointTimer = DateTime.Now.Second; //what is the function of the timer, I get you are converting to degree, but be explicit in commenting for future sake
            int[] movingCircleCoord = new int[2]; // temporary storage for moving circle co-ordinate
            movingCircleCoord = movingPointCoord(movingPointTimer, (int)path_diameter/2); // what is the function doing here

            //set new coordinates for moving circle
            moving_coord_xy_translator.X = movingCircleCoord[0];  // for moving circle X co-ordinate
            moving_coord_xy_translator.Y = movingCircleCoord[1]; //  for moving circle Y co-ordinate


            //moving circle coordinate label indicator, to be removed later on
            moving_circle.Content = "Moving Circle Point: " + "(" + moving_coord_xy_translator.X + "," + moving_coord_xy_translator.Y + ")"; // indicator for the moving circle on screen

            if (goUp  && effector_coord_xy_translator.Y > -270)
            {      
                effector_coord_xy_translator.Y -= speed;
                // if go up is true and controlled circle co-ordinate is greater then -270
            }
            if (goDown && effector_coord_xy_translator.Y < 270)
            {
                // if go down is true and and controlled circle co-ordinate is less then 270
                effector_coord_xy_translator.Y += speed; 
            }
            if (goLeft && effector_coord_xy_translator.X > -340)
            {
                // if go left is true and controlled circle co-ordinate is greater then -340
             
                effector_coord_xy_translator.X -= speed;
               
            }
            if (goRight &&  effector_coord_xy_translator.X < 340 )
            {
                effector_coord_xy_translator.X += speed;
                // if go right is true and controlled circle co-ordinate is less then 340
            }
        }
    }
}
