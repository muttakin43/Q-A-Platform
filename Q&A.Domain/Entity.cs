using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q_A.Domain
{
    public class Entity
    {
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
