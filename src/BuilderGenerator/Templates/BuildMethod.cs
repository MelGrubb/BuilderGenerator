
        public override {{TargetClassFullName}} Build()
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new System.Lazy<{{TargetClassFullName}}>(new {{TargetClassFullName}}());
            }

{{Setters}}

            PostProcess(Object.Value);

            return Object.Value;
        }
