using Collisions;
using DotsFactories;
using Particles;
using Particles.Files;
using Particles.Loading;
using SceneOperations;
using Zenject;

public class DependencyInjectionInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Bind<IParticleRecorder,ParticleRecorder>();
        Bind<IAppStateFactory,AppStateFactory>();
        Bind<IParticleFinder,ParticleFinder>();      
        Bind<IPatternFinder,PatternFinder>();

        Bind<IDuplicatePatternFinder,DuplicatePatternFinder>();
        Bind<ICollisionChecker,CollisionChecker>();
        Bind<IDotConnector,DotConnector>();
        Bind<ICollisionDotsFinder,CollisionDotsFinder>();
        Bind<IParticleGrouper,ParticleGrouper>();
        Bind<IParticleNamer,ParticleNamer>();
        Bind<IParticlePatternChecker,ParticlePatternChecker>();
        Bind<IHistoryLineCreator,HistoryLineCreator>();
        Bind<IParticleSaver,ParticleSaver>();
        Bind<IDotFactory, DotFactory>();
        Bind<IRandomDotListFactory, RandomDotListFactory>();
        Bind<IQuatonFactory, QuatonFactory>();
        Bind<INamedDotsFactory, NamedDotsFactory>();
        Bind<IParticleCollidingGroupLoader, ParticleCollidingGroupLoader>();
        Bind<ISceneResetter, SceneResetter>();
        Bind<IFrozenParticleConverter, FrozenParticleConverter>();
        Bind<IParticleRotator, ParticleRotator>();
        Bind<IParticleLoader, ParticleLoader>();
        Bind<ILargeParticlesLoader, LargeParticlesLoader>();
        Bind<ICenterCollidingDotsFactory, CenterCollidingDotsFactory>();

    }

    private ConcreteIdArgConditionCopyNonLazyBinder Bind<TInterface, TClass>() where TClass : TInterface
    {
        return Container.Bind<TInterface>().To<TClass>().AsSingle();
    }
}