using Contracts;
using Essence;

namespace App_Model
{
    /// <summary>Выбирает программу, сохраняет её тип и выдаёт результат спортсмену.</summary>
    public class TrainingPlanService : ITrainingPlanLogic
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFitnessIndexCalculator _indexCalculator;
        private readonly IReadOnlyList<ITrainingPlanRule> _rules;

        /// <summary>Создаёт сервис с правилом расчёта индекса и упорядоченными правилами программ.</summary>
        public TrainingPlanService(IUnitOfWork unitOfWork,
            IFitnessIndexCalculator indexCalculator, IEnumerable<ITrainingPlanRule> rules)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _indexCalculator = indexCalculator ?? throw new ArgumentNullException(nameof(indexCalculator));
            _rules = rules?.ToList() ?? throw new ArgumentNullException(nameof(rules));
            if (_rules.Count == 0 || _rules.Any(rule => rule == null))
                throw new ArgumentException("Список правил программы должен содержать правила", nameof(rules));
        }

        public string PersonalTraining(Athlete athlete)
        {
            if (athlete == null)
                return "Атлет не найден!";

            var (bmi, fitnessIndex) = _indexCalculator.Calculate(athlete);
            ITrainingPlanRule rule = _rules.FirstOrDefault(rule => rule.Matches(athlete, fitnessIndex))
                ?? throw new InvalidOperationException("Для спортсмена не найдена программа тренировок");
            TrainingPlanDefinition plan = rule.Create(athlete);

            athlete.TypePersonalTraining = plan.RecommendedType;

            Athlete? athleteInDb = _unitOfWork.Athletes.ReadById(athlete.Id);
            if (athleteInDb != null)
            {
                athleteInDb.TypePersonalTraining = plan.RecommendedType;
                _unitOfWork.Athletes.Update(athleteInDb);
                _unitOfWork.Save();
            }

            return
                "=== ИНДИВИДУАЛЬНЫЙ РАСЧЕТ ПРОГРАММЫ ===" +
                Environment.NewLine +
                $"Атлет: {athlete.FullName} ({athlete.Gendre})" +
                Environment.NewLine +
                $"Параметры: Возраст — {athlete.Age} лет | " +
                $"Рост — {athlete.Height} см | " +
                $"Вес — {athlete.Weight} кг" +
                Environment.NewLine +
                $"ИМТ: {bmi:F1} | " +
                $"Индекс нагрузки (по формуле): {fitnessIndex:F1}" +
                Environment.NewLine +
                $"Предпочтение атлета: {athlete.TrainingType}" +
                Environment.NewLine +
                Environment.NewLine +
                "--------------------------------------------------" +
                Environment.NewLine +
                $"НАЗНАЧЕННАЯ ПРОГРАММА: {plan.Title}" +
                Environment.NewLine +
                "--------------------------------------------------" +
                Environment.NewLine +
                $"🎯 Цель: {plan.Goal}" +
                Environment.NewLine +
                $"📅 График: {plan.Schedule}" +
                Environment.NewLine +
                "📋 Содержание тренировок:" +
                Environment.NewLine +
                plan.Details +
                Environment.NewLine +
                Environment.NewLine +
                $"Рекомендованный тип специализации тренера: " +
                $"{plan.RecommendedType}";
        }
    }
}
