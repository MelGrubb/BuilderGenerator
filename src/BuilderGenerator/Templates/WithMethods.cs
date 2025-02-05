
{{PropertyComment}}
        public {{BuilderClassName}} With{{PropertyName}}({{PropertyType}} value)
        {
            return With{{PropertyName}}(() => value);
        }

{{PropertyComment}}
        public {{BuilderClassName}} With{{PropertyName}}(System.Func<{{PropertyType}}> func)
        {
            {{PropertyName}} = new System.Lazy<{{PropertyType}}>(func);
            return this;
        }

{{PropertyComment}}
        public {{BuilderClassName}} Without{{PropertyName}}()
        {
            {{PropertyName}} = new System.Lazy<{{PropertyType}}>(() => default({{PropertyType}}));
            return this;
        }