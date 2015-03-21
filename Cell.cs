using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee
{
    public interface ICell<T> : INotifyPropertyChanged
    {
        T Value { get; set; }
    }

    public class Cell<T> : ICell<T>
    {
        private T value;

        public Cell( T initialValue = default(T) )
        {
            this.value = initialValue;
        }

        public virtual T Value
        {
            get
            {
                return value;
            }
            set
            {
                if ( !AreEqual( this.value, value ) )
                {
                    this.value = value;

                    NotifyObservers();
                }
            }
        }

        private static bool AreEqual( T x, T y )
        {
            if ( x == null )
            {
                return y == null;
            }
            else
            {
                return x.Equals( y );
            }
        }

        public void NotifyObservers()
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( "Value" ) );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public static class Derived
    {
        private static void RegisterObserver<T, R>(Derived<R> derived, ICell<T> cell )
        {
            cell.PropertyChanged += ( sender, args ) => derived.Refresh();
        }

        public static ICell<R> Create<T, R>( ICell<T> cell, Func<T, R> function )
        {
            var derived = new Derived<R>( () => function( cell.Value ) );

            RegisterObserver( derived, cell );

            return derived;
        }

        public static ICell<R> Create<T1, T2, R>( ICell<T1> c1, ICell<T2> c2, Func<T1, T2, R> function )
        {
            var derived = new Derived<R>( () => function( c1.Value, c2.Value ) );

            RegisterObserver( derived, c1 );
            RegisterObserver( derived, c2 );

            return derived;
        }

        public static ICell<R> Create<T1, T2, T3, R>( ICell<T1> c1, ICell<T2> c2, ICell<T3> c3, Func<T1, T2, T3, R> function )
        {
            var derived = new Derived<R>( () => function( c1.Value, c2.Value, c3.Value ) );

            RegisterObserver( derived, c1 );
            RegisterObserver( derived, c2 );
            RegisterObserver( derived, c3 );

            return derived;
        }

        public static ICell<R> Create<T1, T2, T3, T4, R>( ICell<T1> c1, ICell<T2> c2, ICell<T3> c3, ICell<T4> c4, Func<T1, T2, T3, T4, R> function )
        {
            var derived = new Derived<R>( () => function( c1.Value, c2.Value, c3.Value, c4.Value ) );

            RegisterObserver( derived, c1 );
            RegisterObserver( derived, c2 );
            RegisterObserver( derived, c3 );
            RegisterObserver( derived, c4 );

            return derived;
        }

        public static ICell<R> Create<T1, T2, T3, T4, T5, R>( ICell<T1> c1, ICell<T2> c2, ICell<T3> c3, ICell<T4> c4, ICell<T5> c5, Func<T1, T2, T3, T4, T5, R> function )
        {
            var derived = new Derived<R>( () => function( c1.Value, c2.Value, c3.Value, c4.Value, c5.Value ) );

            RegisterObserver( derived, c1 );
            RegisterObserver( derived, c2 );
            RegisterObserver( derived, c3 );
            RegisterObserver( derived, c4 );
            RegisterObserver( derived, c5 );

            return derived;
        }

        public static ICell<R> Create<T, R>(IEnumerable<ICell<T>> cells, Func<IEnumerable<T>, R> function)
        {
            var derived = new Derived<R>( () => function( cells.Select( cell => cell.Value ) ) );

            foreach ( var cell in cells )
            {
                RegisterObserver( derived, cell );
            }

            return derived;
        }
    }

    public class Derived<T> : Cell<T>
    {
        private readonly Func<T> function;

        public Derived( Func<T> function )
            : base( function() )
        {
            this.function = function;
        }

        public void Refresh()
        {
            base.Value = function();
        }

        public override T Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                throw new InvalidOperationException();
            }
        }
    }
}
