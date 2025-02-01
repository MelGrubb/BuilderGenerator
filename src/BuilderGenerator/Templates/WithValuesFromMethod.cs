        /// <summary>Populates this instance with values from the provided example.</summary>
        /// <remarks>This is a shallow clone, and does not traverse the example object creating builders for its properties.</remarks>
        public {{BuilderClassName}} WithValuesFrom({{TargetClassFullName}} example)
        {
{{WithValuesFromSetters}}

            return this;
        }