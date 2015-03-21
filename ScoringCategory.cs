using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee
{
    public abstract class ScoringCategory
    {
        public abstract string Name { get; }

        public abstract int Score( DiceRoll roll );
    }

    public class NumberCategory : ScoringCategory
    {
        private readonly int n;

        public NumberCategory( int n )
        {
            this.n = n;
        }

        public override string Name
        {
            get
            {
                return n.ToString();
            }
        }

        public override int Score( DiceRoll roll )
        {
            return roll.AsFrequencyTable()[n] * n;
        }
    }

    public abstract class NOfAKindCategory : ScoringCategory
    {
        private readonly int n;

        protected NOfAKindCategory( int n )
        {
            this.n = n;
        }
        
        protected bool Matches( DiceRoll roll )
        {
            return roll.AsFrequencyTable().Frequencies.Any( f => f >= n );
        }
    }

    public class ThreeOfAKindCategory : NOfAKindCategory
    {
        public ThreeOfAKindCategory()
            : base( 3 )
        {
            // NOP
        }

        public override string Name
        {
            get { return "Three of a Kind"; }
        }

        public override int Score( DiceRoll roll )
        {
            return Matches(roll) ? roll.AsList().Sum() : 0;
        }
    }

    public class FourOfAKindCategory : NOfAKindCategory
    {
        public FourOfAKindCategory()
            : base( 4 )
        {
            // NOP
        }

        public override string Name
        {
            get { return "Four of a Kind"; }
        }

        public override int Score( DiceRoll roll )
        {
            return Matches(roll) ? roll.AsList().Sum() : 0;
        }
    }

    public class YahtzeeCategory : NOfAKindCategory
    {
        public YahtzeeCategory()
            : base( 5 )
        {
            // NOP
        }

        public override string Name
        {
            get { return "Yahtzee"; }
        }

        public override int Score( DiceRoll roll )
        {
            return Matches(roll) ? 50 : 0;
        }
    }

    public class FullHouseCategory : ScoringCategory
    {
        public override string Name
        {
            get
            {
                return "Full House";
            }
        }

        private bool Matches( DiceRoll roll )
        {
            var freqs = new HashSet<int>( roll.AsFrequencyTable().Frequencies );

            return ( freqs.Contains( 2 ) && freqs.Contains( 3 ) ) || freqs.Contains( 5 );
        }

        public override int Score( DiceRoll roll )
        {
            return Matches( roll ) ? 25 : 0;
        }
    }

    public abstract class StraightCategory : ScoringCategory
    {
        protected bool Matches( DiceRoll roll, int length )
        {
            // Remove duplicates by going through set
            var values = roll.AsSet().ToList();
            values.Sort();

            var last = -1;
            var count = 0;

            foreach ( var val in values )
            {
                if ( val - last == 1 )
                {
                    count++;

                    if ( count == length - 1 )
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }

                last = val;
            }

            return false;
        }
    }

    public class SmallStraightCategory : StraightCategory
    {
        public override string Name
        {
            get
            {
                return "Small Straight";
            }
        }

        public override int Score( DiceRoll roll )
        {
            return Matches( roll, 4 ) ? 30 : 0;
        }
    }

    public class LargeStraightCategory : StraightCategory
    {
        public override string Name
        {
            get
            {
                return "Large Straight";
            }
        }

        public override int Score( DiceRoll roll )
        {
            return Matches( roll, 5 ) ? 40 : 0;
        }
    }

    public class ChanceCategory : ScoringCategory
    {
        public override string Name
        {
            get { return "Chance"; }
        }

        public override int Score( DiceRoll roll )
        {
            return roll.AsList().Sum();
        }
    }

    public class FrequencyTable<T>
    {
        private readonly Dictionary<T, int> counts;

        public FrequencyTable()
            : this( Enumerable.Empty<T>() )
        {
            // NOP
        }

        public FrequencyTable( IEnumerable<T> xs )
        {
            counts = new Dictionary<T, int>();

            AddAll( xs );
        }

        public void Add( T x )
        {
            counts[x] = this[x] + 1;
        }

        public void AddAll( IEnumerable<T> xs )
        {
            foreach ( var x in xs )
            {
                Add( x );
            }
        }

        public int this[T item]
        {
            get
            {
                return counts.ContainsKey( item ) ? counts[item] : 0;
            }
        }

        public IEnumerable<int> Frequencies
        {
            get
            {
                return counts.Values;
            }
        }
    }
}
