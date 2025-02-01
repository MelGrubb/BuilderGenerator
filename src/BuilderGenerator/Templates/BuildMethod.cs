        public override {{TargetClassFullName}} Build()
        {
            if ({{TargetClassName}}?.IsValueCreated != true)
            {
                {{TargetClassName}} = new System.Lazy<{{TargetClassFullName}}>(() =>
                {
                    var result = new {{TargetClassFullName}}
                    {
{{Setters}}
                    };

                    return result;
                });

                PostProcess({{TargetClassName}}.Value);
            }

            return {{TargetClassName}}.Value;
        }
