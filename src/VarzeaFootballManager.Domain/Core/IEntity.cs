namespace VarzeaFootballManager.Domain.Core
{
    public interface IEntity
    {
        string Id { get; set; }

        System.DateTime CreatedAt { get; set; }

        System.DateTime? ModifiedAt { get; set; }
    }
}