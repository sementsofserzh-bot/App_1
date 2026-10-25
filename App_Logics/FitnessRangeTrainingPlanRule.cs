using Essence;

namespace App_Model
{
    /// <summary>Программа, применимая к диапазону значений индекса.</summary>
    public class FitnessRangeTrainingPlanRule : ITrainingPlanRule
    {
        private readonly double? _fromInclusive;
        private readonly double? _toExclusive;
        private readonly string _title;
        private readonly string _goal;
        private readonly string _schedule;
        private readonly string _details;
        private readonly TrainingType _maleType;
        private readonly TrainingType _femaleType;

        /// <summary>Создаёт правило программы для заданного диапазона индекса.</summary>
        public FitnessRangeTrainingPlanRule(double? fromInclusive, double? toExclusive,
            string title, string goal, string schedule, string details,
            TrainingType maleType, TrainingType femaleType)
        {
            if (fromInclusive.HasValue && toExclusive.HasValue && fromInclusive >= toExclusive)
                throw new ArgumentException("Некорректный диапазон индекса");

            _fromInclusive = fromInclusive;
            _toExclusive = toExclusive;
            _title = title ?? throw new ArgumentNullException(nameof(title));
            _goal = goal ?? throw new ArgumentNullException(nameof(goal));
            _schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
            _details = details ?? throw new ArgumentNullException(nameof(details));
            _maleType = maleType;
            _femaleType = femaleType;
        }

        public bool Matches(Athlete athlete, double fitnessIndex) =>
            (double.IsNaN(fitnessIndex) && !_toExclusive.HasValue) ||
            ((!_fromInclusive.HasValue || fitnessIndex >= _fromInclusive.Value) &&
             (!_toExclusive.HasValue || fitnessIndex < _toExclusive.Value));

        public TrainingPlanDefinition Create(Athlete athlete) =>
            new(_title, _goal, _schedule, _details,
                athlete.Gendre == Gendre.М ? _maleType : _femaleType);
    }
}
