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


        public float[] movingPointCoord(float radius, float angleInDegrees, PointF origin) // This is the new ellipse point function 
        {

            //  angleInDegrees:  step angle for the moving circle
            //  radius: the radius at with the moving circle moves around the circumference
            float[] coord = new float[2]; // array for the x and y co-ordinates
            coord[0] = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180)) + origin.X;  // the x co-ordinate
            coord[1] = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180)) + origin.Y; // the y co-ordinate 

            return coord; // it returens the array of the two co-ordinates
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

           

            float[] movingCircleCoord = new float[2]; // temporary storage for moving circle co-ordinate
            movingCircleCoord = movingPointCoord((int)path_diameter/2, angle, org); // the function return the x and y co-ordinate for the moving circle 
            if (angle <= 360 || angle >= 0)  //condition that controls the moving circle
            {
                angle += 0.5f;
                if (angle % 2 == 0)
                {
                    moving_coord_ellipse.Fill = System.Windows.Media.Brushes.Black;
                    moving_coord_ellipse.Stroke = System.Windows.Media.Brushes.Black;
                }
                else
                {
                    moving_coord_ellipse.Fill = System.Windows.Media.Brushes.White;
                    moving_coord_ellipse.Stroke = System.Windows.Media.Brushes.White;
                }
            }


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
