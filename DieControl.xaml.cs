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

namespace Yahtzee
{
    /// <summary>
    /// Interaction logic for DieControl.xaml
    /// </summary>
    public partial class DieControl : UserControl
    {
        public DieControl()
        {
            InitializeComponent();

            this.SizeChanged += ( obj, args ) => DrawFace();
        }

        public int DieValue
        {
            get { return (int) GetValue( DieValueProperty ); }
            set { SetValue( DieValueProperty, value ); }
        }

        private void OnDieValueChanged()
        {
            DrawFace();
        }

        private void DrawFace()
        {
            switch ( DieValue )
            {
                case 1:
                    DrawOne();
                    break;

                case 2:
                    DrawTwo();
                    break;

                case 3:
                    DrawThree();
                    break;

                case 4:
                    DrawFour();
                    break;

                case 5:
                    DrawFive();
                    break;

                case 6:
                    DrawSix();
                    break;

                default:
                    DrawError();
                    break;
            }
        }

        private void DrawError()
        {
            canvas.Children.Clear();
        }

        private void DrawOne()
        {
            canvas.Children.Clear();

            AddEllipse( .5, .5 );
        }

        private void DrawTwo()
        {
            canvas.Children.Clear();

            AddEllipse( .25, .25 );
            AddEllipse( .75, .75 );
        }

        private void DrawThree()
        {
            canvas.Children.Clear();

            AddEllipse( .25, .75 );
            AddEllipse( .5, .5 );
            AddEllipse( .75, .25 );
        }

        private void DrawFour()
        {
            canvas.Children.Clear();

            AddEllipse( .25, .25 );
            AddEllipse( .25, .75 );
            AddEllipse( .75, .25 );
            AddEllipse( .75, .75 );
        }

        private void DrawFive()
        {
            canvas.Children.Clear();

            AddEllipse( .25, .25 );
            AddEllipse( .25, .75 );
            AddEllipse( .5, .5 );
            AddEllipse( .75, .25 );
            AddEllipse( .75, .75 );
        }

        private void DrawSix()
        {
            canvas.Children.Clear();

            AddEllipse( .25, .25 );
            AddEllipse( .25, .5 );
            AddEllipse( .25, .75 );
            AddEllipse( .75, .25 );
            AddEllipse( .75, .5 );
            AddEllipse( .75, .75 );
        }

        private void AddEllipse( double x, double y )
        {
            var ellipse = new Ellipse();
            ellipse.Fill = Brushes.Black;
            ellipse.Width = 5;
            ellipse.Height = 5;

            Canvas.SetLeft( ellipse, x * Width - ellipse.Width / 2 );
            Canvas.SetTop( ellipse, y * Height - ellipse.Height / 2 );

            canvas.Children.Add( ellipse );
        }

        public static readonly DependencyProperty DieValueProperty =
            DependencyProperty.Register( "DieValue", typeof( int ), typeof( DieControl ), new PropertyMetadata( 1, DieValueChanged ) );

        public static void DieValueChanged( DependencyObject obj, DependencyPropertyChangedEventArgs args )
        {
            var dieControl = (DieControl) obj;

            dieControl.OnDieValueChanged();
        }
    }
}
