using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Core
{
    public class Conveyor : Building
    {
        public ConveyorComponent? Component => this.GetComponent<ConveyorComponent>();
        public override CharacterType Type => CharacterType.Conveyor;
        public override int Height => 1;

        public Conveyor(Context context, int alliance) : base(context, alliance) { }

        protected override void InitComponents()
        {
            this.SetComponent(new ConveyorComponent(this));
        }

        public override Schema.Entity BuildSchemaObject()
        {
            return new Schema.Conveyor();
        }
    }
}