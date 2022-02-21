using System;

namespace Langate.FacialRecognition.Model
{
    public abstract class Entity
    {
        protected Entity()
        {
            CreatedDate = DateTime.UtcNow;
        }

        public DateTime CreatedDate { get; private set; }
    }
}