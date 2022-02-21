using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Langate.FacialRecognition.Model
{
    public class Token: Entity
    {
        public int TokenId { get; set; }
        public string TokenValue { get; set; } = Guid.NewGuid().ToString();
        public bool Used { get; set; }

        public virtual IList<ApiCall> ApiCalls { get; set; } = new List<ApiCall>();

        public virtual IList<Invite> Invites { get; set; } = new List<Invite>();
    }
}
