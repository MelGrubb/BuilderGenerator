
        public {{BuilderClassName}} With{{PropertyName}}({{PropertyType}} value)
        {
            return With{{PropertyName}}(() => value);
        }

        public {{BuilderClassName}} With{{PropertyName}}(System.Func<{{PropertyType}}> func)
        {
            {{PropertyName}} = new System.Lazy<{{PropertyType}}>(func);
            return this;
        }

        public {{BuilderClassName}} Without{{PropertyName}}()
        {
            {{PropertyName}} = new System.Lazy<{{PropertyType}}>(() => default({{PropertyType}}));
            return this;
        }