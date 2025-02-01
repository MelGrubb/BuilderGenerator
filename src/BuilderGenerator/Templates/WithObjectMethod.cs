        /// <summary>Sets the object to be returned by this instance.</summary>
        /// <param name="value">The object to be returned.</param>
        /// <returns>A reference to this builder instance.</returns>
        public {{BuilderClassName}} With{{TargetClassName}}({{TargetClassFullName}} value)
        {
            {{TargetClassName}} = new System.Lazy<{{TargetClassFullName}}>(() => value);
            WithValuesFrom(value);

            return this;
        }
