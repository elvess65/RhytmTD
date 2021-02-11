using System;
using CoreFramework;

namespace RhytmTD.Data.Models
{
    public class UpdateModel : BaseModel
    {
        public Action<float> OnUpdate;
    }
}
