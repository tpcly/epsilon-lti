using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Epsilon.Abstractions.Data;

public abstract record Entity<TKey>(TKey? Id = default) : IEntity<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey? Id { get; set; } = Id;

    object? IEntity.Id
    {
        get => Id;
        init => Id = value is TKey key
            ? key
            : default;
    }
}