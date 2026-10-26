using App_Model;
using Contracts;
using DataAccessLayer;
using Essence;
using Ninject;
using Ninject.Modules;

namespace App_Composition
{
    /// <summary>Связывает контракты приложения с реализациями.</summary>
    public class ApplicationModule : NinjectModule
    {
        /// <summary>Регистрирует хранилище и сервисы бизнес-логики в контейнере.</summary>
        public override void Load()
        {
            // Одна единица работы и одни экземпляры репозиториев на время приложения.
            Bind<IUnitOfWork>().ToMethod(_ => new DapperUnitOfWork()).InSingletonScope();
            Bind<IRepository<Trainer>>()
                .ToMethod(context => context.Kernel.Get<IUnitOfWork>().Trainers)
                .InSingletonScope();
            Bind<IRepository<Athlete>>()
                .ToMethod(context => context.Kernel.Get<IUnitOfWork>().Athletes)
                .InSingletonScope();

            Bind<TrainerService>().ToSelf().InSingletonScope();
            Bind<ITrainerService>()
                .ToMethod(context => context.Kernel.Get<TrainerService>())
                .InSingletonScope();
            Bind<ITrainerEditLogic>()
                .ToMethod(context => context.Kernel.Get<TrainerService>())
                .InSingletonScope();

            Bind<AthleteService>().ToSelf().InSingletonScope();
            Bind<IAthleteService>()         
                .ToMethod(context => context.Kernel.Get<AthleteService>())
                .InSingletonScope();
            Bind<IAthleteCreateLogic>()
                .ToMethod(context => context.Kernel.Get<AthleteService>())
                .InSingletonScope();

            Bind<IRegistrationLogic>().To<RegistrationService>().InSingletonScope();
            Bind<IFitnessIndexCalculator>()
                .To<DefaultFitnessIndexCalculator>().InSingletonScope();
            foreach (ITrainingPlanRule rule in DefaultTrainingPlanRules.Create())
                Bind<ITrainingPlanRule>().ToConstant(rule);
            Bind<ITrainingPlanLogic>().To<TrainingPlanService>().InSingletonScope();

            Bind<ITrainerMatchCalculator>()
                .To<DefaultTrainerMatchCalculator>().InSingletonScope();
            Bind<ITrainerMatchingLogic>()
                .To<TrainerMatchingService>().InSingletonScope();
            Bind<ITrainerRatingCalculator>()
                .To<DefaultTrainerRatingCalculator>().InSingletonScope();
            Bind<ITrainerRatingLogic>()
                .To<TrainerRatingService>().InSingletonScope();

            Bind<InitialDataSeeder>().ToSelf().InSingletonScope();
            Bind<Logics>().ToSelf().InSingletonScope();
            Bind<ILogics>()
                .ToMethod(context => context.Kernel.Get<Logics>())
                .InSingletonScope();
            Bind<IEmployeeLogic>()
                .ToMethod(context => context.Kernel.Get<Logics>())
                .InSingletonScope();
            Bind<IUserLogic>()
                .ToMethod(context => context.Kernel.Get<Logics>())
                .InSingletonScope();
            Bind<IConsoleUserLogic>()
                .ToMethod(context => context.Kernel.Get<Logics>())
                .InSingletonScope();
        }
    }
}
