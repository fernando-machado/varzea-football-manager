namespace VarzeaFootballManager.Domain.Core
{
    public abstract class AggregateRoot : Entity
    {

    }








    //TODO: O restante do código abaxo faz parte do modulo de Domain Events do curso DDD in Pratice do Vladimir Khorikov na pluralsight
    //https://www.pluralsight.com/courses/domain-driven-design-in-practice
    //http://enterprisecraftsmanship.com/2016/01/22/domain-driven-design-in-practice-pluralsight-course/
    //public abstract class AggregateRoot : Entity
    //{
    //    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    //    public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    //    protected virtual void AddDomainEvent(IDomainEvent newEvent)
    //    {
    //        _domainEvents.Add(newEvent);
    //    }

    //    public virtual void ClearEvents()
    //    {
    //        _domainEvents.Clear();
    //    }
    //}

    //public interface IDomainEvent
    //{
    //}

    //public interface IHandler<T> where T : IDomainEvent
    //{
    //    void Handle(T domainEvent);
    //}

    //public static class DomainEvents
    //{
    //    private static List<Type> _handlers;

    //    public static void Init()
    //    {
    //        _handlers = Assembly.GetExecutingAssembly()
    //            .GetTypes()
    //            .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IHandler<>)))
    //            .ToList();
    //    }

    //    public static void Dispatch(IDomainEvent domainEvent)
    //    {
    //        foreach (Type handlerType in _handlers)
    //        {
    //            bool canHandleEvent = handlerType.GetInterfaces()
    //                .Any(x => x.IsGenericType
    //                    && x.GetGenericTypeDefinition() == typeof(IHandler<>)
    //                    && x.GenericTypeArguments[0] == domainEvent.GetType());

    //            if (canHandleEvent)
    //            {
    //                dynamic handler = Activator.CreateInstance(handlerType);
    //                handler.Handle((dynamic)domainEvent);
    //            }
    //        }
    //    }
    //}
}
