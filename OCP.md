# Расширение бизнес-правил

`TrainingPlanService` получает расчёт индекса (`IFitnessIndexCalculator`) и упорядоченный
список правил (`ITrainingPlanRule`). Первое подходящее правило создаёт программу.
Пять исходных программ находятся в `DefaultTrainingPlanRules`. Новое правило можно
поставить перед ними при создании сервиса, не изменяя сам сервис:

```csharp
IEnumerable<ITrainingPlanRule> rules =
    new ITrainingPlanRule[] { new MyTrainingPlanRule() }
        .Concat(DefaultTrainingPlanRules.Create());
var plans = new TrainingPlanService(unitOfWork, new DefaultFitnessIndexCalculator(), rules);
```

Расчёты рейтинга и соответствия тренера спортсмену заменяются реализациями
`ITrainerRatingCalculator` и `ITrainerMatchCalculator`. В EF-репозиторий при создании
передаётся способ загрузки связанных данных; его код не содержит проверок конкретных
типов сущностей.

