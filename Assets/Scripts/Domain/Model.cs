using System;

namespace TrenchWarfare.Domain {
    public abstract class Model: IModel {
        Guid id;
        public Guid Id { get => id; protected set => id = value; }

        protected Model() {
            id = NewId();
        }

        protected Guid NewId() {
            return Guid.NewGuid();
        }
    }
}
