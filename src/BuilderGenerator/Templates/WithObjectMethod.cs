        /// <summary>Sets the object to be returned by this instance.</summary>
        /// <param name="value">The object to be returned.</param>
        /// <returns>A reference to this builder instance.</returns>
        public override Builder<{{TargetClassFullName}}> WithObject({{TargetClassFullName}} value)
        {
            base.WithObject(value);

{{WithObjectMethodSetters}}

            return this;
        }