using System;
using System.Collections.Generic;
using System.Text;

namespace Langate.FacialRecognition.Mobile.Models.Local
{
    public class PageResponseModel<TEntity>
    {
        public TEntity Entity { get; set; }
        public bool Destroyed { get; set; }
    }
}
