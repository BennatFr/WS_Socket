using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
    public class People {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? id { get; set; }

        public People() {
        }

        public People(string firstName, string lastName) {
            this.firstName = firstName;
            this.lastName = lastName;
        }
    }
}
