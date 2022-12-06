#region Creation with DI, interfaces and lifetime

var dependencyContainer = DependencyRulesContainer.Instance;
var dependencyResolver = DependencyResolver.Instance;

dependencyContainer.AddSingletonDependency<IMessageBuilder, MessageBuilder>();
dependencyContainer.AddSingletonDependency<IMessageSender, MessageSender>();
dependencyContainer.AddSingletonDependency<IRegistrationService, RegistrationService>();

//dependencyContainer.AddTransientDependency<IMessageBuilder, MessageBuilder>();
//dependencyContainer.AddTransientDependency<IMessageSender, MessageSender>();
//dependencyContainer.AddTransientDependency<IRegistrationService, RegistrationService>();

var registrationService1 = dependencyResolver.GetService<IRegistrationService>();
registrationService1.Register("boo");
var registrationService2 = dependencyResolver.GetService<IRegistrationService>();
registrationService2.Register("boo");
var registrationService3 = dependencyResolver.GetService<IRegistrationService>();
registrationService3.Register("boo");

#endregion


#region Class for DI

public class DependencyResolver // IServiceProvider
{
    private readonly DependencyRulesContainer _container;
    private readonly Dictionary<DependencyRule, object> _implementations;

    public static DependencyResolver Instance { get; }

    static DependencyResolver()
    {
        Instance ??= new DependencyResolver();
    }

    public DependencyResolver()
    {
        _container = DependencyRulesContainer.Instance;
        _implementations = new();
    }

    public T GetService<T>() => (T) GetService(typeof(T));

    private object GetService(Type type)
    {
        var dependencyRule = _container.GetDependency(type);
        var isServiceImplemented = _implementations.TryGetValue(dependencyRule, out var service);
        if (isServiceImplemented)
            return service;

        var instance = GetInstance(dependencyRule);
        return RegisterInstance(dependencyRule, instance);
    }

    private object GetInstance(DependencyRule dependencyRule)
    {
        var serviceConstructor = dependencyRule.ConcreteClass.GetConstructors().Single();
        var constructorParameterTypes = serviceConstructor.GetParameters().Select(p => p.ParameterType).ToArray();
        var constructorParameters = new List<object>();

        if (constructorParameterTypes.Length > 0)
        {
            for (int index = 0; index < constructorParameterTypes.Length; index++)
            {
                var paramType = constructorParameterTypes[index];

                var parameter = GetService(constructorParameterTypes[index]);
                constructorParameters.Add(parameter);
            }

            return Activator.CreateInstance(dependencyRule.ConcreteClass, constructorParameters);
        }

        return Activator.CreateInstance(dependencyRule.ConcreteClass);
    }

    private object RegisterInstance(DependencyRule dependencyRule, object instance)
    {
        var insertInstance = dependencyRule.Abstraction is not null ? 
            dependencyRule.Abstraction.CastFrom(instance) : instance;

        if (dependencyRule.Lifetime == Lifetime.Singleton)
            _implementations.Add(dependencyRule, insertInstance);

        return insertInstance;
    }
}

public static class ConvertHelper
{
    public static T CastFrom<T>(this T castTo, object castFrom) => (T)castFrom;
}

public class DependencyRulesContainer // IServiceCollection
{
    private readonly List<DependencyRule> _dependencyRules;

    public static DependencyRulesContainer Instance { get; }

    static DependencyRulesContainer()
    {
        Instance ??= new DependencyRulesContainer();
    }

    private DependencyRulesContainer()
    {
        _dependencyRules = new();
    }

    public DependencyRule GetDependency(Type type)
    {
        if (type.IsInterface)
            return _dependencyRules.Single(d => d.Abstraction is not null && d.Abstraction.Name == type.Name);
        return _dependencyRules.Single(d => d.ConcreteClass.Name == type.Name && d.Abstraction is null);
    }

    public void AddSingletonDependency<TConcrete>() =>
        _dependencyRules.Add(new DependencyRule(typeof(TConcrete), Lifetime.Singleton));

    public void AddSingletonDependency<TAbstract, TConcrete>() =>
        _dependencyRules.Add(new DependencyRule(typeof(TAbstract), typeof(TConcrete), Lifetime.Singleton));

    public void AddTransientDependency<TConcrete>() =>
        _dependencyRules.Add(new DependencyRule(typeof(TConcrete), Lifetime.Transient));

    public void AddTransientDependency<TAbstract, TConcrete>() =>
        _dependencyRules.Add(new DependencyRule(typeof(TAbstract), typeof(TConcrete), Lifetime.Transient));
}

public class DependencyRule
{
    public Lifetime Lifetime { get; }
    public Type? Abstraction { get; }
    public Type ConcreteClass { get; }

    public DependencyRule(Type concreteImplementation, Lifetime lifetime)
    {
        ConcreteClass = concreteImplementation;
        Lifetime = lifetime;
    }

    public DependencyRule(Type abstraction, Type concreteImplementation, Lifetime lifetime) 
        : this(concreteImplementation, lifetime)
    {
        Abstraction = abstraction;
    }
}

public enum Lifetime
{
    Singleton,
    Transient
}

#endregion

#region Interfaces to instantiate

public interface IMessageBuilder
{
    void GetMessage(string message);
}

public class MessageBuilder : IMessageBuilder
{
    public void GetMessage(string message) => Console.WriteLine($"message: {message}");
}

public interface IMessageSender
{
    void SendMessage(string message);
}

public class MessageSender : IMessageSender
{
    private readonly IMessageBuilder _builder;
    private readonly int _number;

    public MessageSender(IMessageBuilder builder)
    {
        _builder = builder;
        _number = new Random().Next();
    }

    public void SendMessage(string message)
    {
        Console.WriteLine("get user email...");
        _builder.GetMessage(message);
        Console.WriteLine("email sent!");
        Console.WriteLine($"{nameof(MessageBuilder)}: {_number}");
    }
}

public interface IRegistrationService
{
    void Register(string username);
}

public class RegistrationService : IRegistrationService
{
    private readonly IMessageSender _sender;
    private readonly int _number;

    public RegistrationService(IMessageSender sender)
    {
        _sender = sender;
        _number = new Random().Next();
    }

    public void Register(string username)
    {
        Console.WriteLine($"{username} registered");
        _sender.SendMessage($"welcome, {username}");
        Console.WriteLine($"{nameof(RegistrationService)}: {_number}");
    }
}

#endregion