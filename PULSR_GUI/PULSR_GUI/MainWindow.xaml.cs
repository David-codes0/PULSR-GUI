using System;
using System.Collections.Generic;
using System.Drawing;
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



        float angle = 0.0f; // steps angle for the new ellipse
        PointF org = new PointF(0, 0); // origin position for the new ellipse
        float rad = 200; // radius of the new rotationing ellipse
        PointF loc = PointF.Empty; // location of the new rotating circle

       
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

        private int[] movingPointCoord(double val, int hlen)
        {
            //make hlen argument type double as the vawriable it is to take is double and typecasting to int can cause data loss
            /*
             * define what this function does by defining
             * 1. role of input arguments
             * 2. role of returned integer array
             */
            int[] coord = new int[2]; //good use of dynamic memory, thumbs up
            val *= 0.008;   //each minute and second make 6 degree
            double date = DateTime.Now.Millisecond * 3.6;

            if (val >= 0 && val <= 360)
            {
                coord[0] = 0 + (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = 0 - (int)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else if (val >= 360)
            {
                val = val - 360;
                coord[0] = (int)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = (int)(hlen * -Math.Cos(Math.PI * val / 180));

            }
            main_window.Title = " " + val + " " + date;
            return coord;
        }
        public PointF newEllipsePoint(float radius, float angleInDegrees, PointF origin) // This is the new ellipse point function 
        {
            float x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X;
            float y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y;

            return new PointF(x, y);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            /*
             * comment the function of input arguments and
             * function of function itslef
             */

            gui_width = main_window_grid.ActualWidth; // the gui grid width variable
            gui_height = main_window_grid.ActualHeight; // the gui grid height variable

            double path_diameter = gui_height*0.7; // path diameter for flexible conversion of t_mode_circular_path width and height as the gui grid width and height changes
            t_mode_circular_path.Width = path_diameter; // dynamic width for t_mode
            t_mode_circular_path.Height = path_diameter; // dynamic height for t_mode

            //controlled circle coordinate label indicator, to be removed later on
            controlled_Circle.Content = "Controlled Circle Point :" + "(" + effector_coord_xy_translator.X + "," + effector_coord_xy_translator.Y + ")"; // indicator for the controlled circle


            double movingPointTimer = DateTime.Now.Millisecond *12; //what is the function of the timer, I get you are converting to degree, but be explicit in commenting for future sake
            int[] movingCircleCoord = new int[2]; // temporary storage for moving circle co-ordinate
            movingCircleCoord = movingPointCoord(movingPointTimer, (int)path_diameter/2); // the function return the x and y co-ordinate for the moving circle 


         
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

         /// NEW ELLIPSE LOGICS

                loc = newEllipsePoint(rad, angle, org); // stores the points or location gotten from  the newEllipsePoint function  
                new_ellipse_coord_xy_translator.X = (int)(loc.X - (new_ellipse.Width ) + (int)path_diameter/10 );// for moving the new ellipse X co - ordinate
                new_ellipse_coord_xy_translator.Y = (int)(loc.Y - (new_ellipse.Height ) + (int)path_diameter/10 ); // for moving the new ellipse Y co- ordinate



                if (angle < 360)//  condition that controls the new ellipses
                {
                    angle += 1.5f;
                }
                else if(angle == 0)
                {
                    angle = 0;
                }

            




        }

       
    }
    
}
