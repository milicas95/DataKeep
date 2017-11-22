using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBparam
{
    public class DBParam
    {
        private int id;
        private string region;
        private string city;
        private int year;
        private string month;
        private int elEnergySpent;
        private List<int> ids;          //list svih IDjeva u bazi

        public DBParam() { }

        public List<int> IDs { get; set; }

        public int Id { get; set; }

        public string Region { get; set; }

        public string City
        {
            get;
            set;
        }
        public int Year
        {
            get;
            set;
        }
        public string Month
        {
            get;
            set;
        }
        public int ElEnergySpent
        {
            get;
            set;
        }
    }
}
