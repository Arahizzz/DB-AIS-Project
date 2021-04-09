using System;

namespace DBAIS
{
    public class EntityNotFoundException<TEntity, TKey> : Exception
    {
        public TKey Id { get; set; }

        public EntityNotFoundException(TKey id) 
            : base($"Could not find entity {nameof(TEntity)} with id {id}")
        {
            Id = id;
        }
    }
}