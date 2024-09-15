using System;
using System.ComponentModel.DataAnnotations;

namespace Administration.Application.ViewModels
{
    /// <summary>
    /// Base class for models.
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        [ScaffoldColumn(false)]
        public virtual Guid Id { get; set; }

        public virtual object GetId()
        {
            return $"({Id})";
        }
    }
}