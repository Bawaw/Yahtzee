using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Yahtzee
{
    class DiceRollerViewModel {
        private class RollCommand : ICommand
        {
            DiceRollerViewModel viewModel;
            public RollCommand(DiceRollerViewModel drvm)
            {
                viewModel = drvm;
                // Call CanExecuteChanged whenever CanRoll changes
                viewModel.CanRoll.PropertyChanged += (sender, args) =>
                {
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, new EventArgs());
                    }
                };
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                viewModel.PerformRoll();
            }
        }


        public ICell<DiceRoll> DiceRoll { get; set; }
        private IList<DieViewModel> dice;
        public IList<DieViewModel> Dice { get { return dice; } }

        private Cell<int> rollsLeft = new Cell<int>(4);
        public Cell<int> RollsLeft { get { return rollsLeft; } set {rollsLeft = value ;} }

        private ICell<bool> canRoll = new Cell<bool>(true);
        public ICell<bool> CanRoll { get { return canRoll; } set { canRoll = value; } }

        private ICommand roll;
        public ICommand Roll { get { return roll;} }

        public DiceRollerViewModel(){
            
            List<DieViewModel> temp = new List<DieViewModel>(5);
            CanRoll = Derived.Create(RollsLeft, n => n > 0);
            for (int i = 0; i < 5; i++)
                temp.Add(new DieViewModel());
            dice = temp.AsReadOnly();

             
            
            this.DiceRoll = Derived.Create(dice.Select( die => die.DieFace ), ns => new DiceRoll(ns.ToArray()));
            roll = new RollCommand(this);
            PerformRoll();
        }

        private void PerformRoll()
        {
            RollsLeft.Value--;
            if (CanRoll.Value) { 
                foreach (DieViewModel d in Dice)
                    d.PerformRoll();
            }
        }

        public void Reset() {
            rollsLeft.Value = 3;
            PerformRoll();
            foreach (DieViewModel d in Dice)
                d.Keep.Value = false;
        }

    }
    class DieViewModel 
    {
        private static Random random = new Random();
        
        private readonly Cell<int> dieFace = new Cell<int>(1);
        public Cell<int> DieFace { get { return dieFace; }}
        
        private Cell<bool> keep = new Cell<bool>(false);
        public Cell<bool> Keep { get { return keep; } set { keep = value; } }

        public void PerformRoll()
        {
            if (!Keep.Value)
                DieFace.Value = random.Next(6) + 1;
        }
    }

    class ScoreLineViewModel {
        private ScoreLine scoreLine;
        public string CategoryName { get { return scoreLine.Category.Name; ; }}

        public ScoreLineViewModel(ScoreLine scoreline) {
            this.scoreLine = scoreline;
        }
    }

    class ScoreSheetViewModel {
        DiceRollerViewModel diceRollerViewModel;
        private ScoreSheet scoreSheet;
        private IList<ScoreLineViewModel> scoreLines;
        public IList<ScoreLineViewModel> ScoreLines { get { return scoreLines; } set { scoreLines = value;} }


        public ScoreSheetViewModel(ScoreSheet scoreSheet, DiceRollerViewModel diceRollerViewModel){
            this.diceRollerViewModel = diceRollerViewModel;
            this.scoreSheet = scoreSheet;
            scoreLines = new List<ScoreLineViewModel>();
            List<ScoreLineViewModel> _scoreLines = new List<ScoreLineViewModel>();
            foreach (ScoreLine scoreLine in this.scoreSheet.ScoreLines){
                _scoreLines.Add(new ScoreLineViewModel(scoreLine));
            }
            scoreLines = _scoreLines.AsReadOnly();
        }
    }
}
