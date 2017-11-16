using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBparam
{
    public class DBParam
    {
        private static int id=0;     
        private string region;
        private string city;
        private int year;
        private string month;
        private int elEnergySpent;

        public DBParam(string Region, string City, int Year, string Month, int ElEnergySpent) {
            id = this.ID;
            region = Region;
            city = City;
            year = Year;
            month = Month;
            elEnergySpent = ElEnergySpent;
        }

        public int ID
        {
            set { id++; }

            get { return id; }
        }

        public string Region
        {
            get { return region; }
        }

        public string City
        {
            get { return city; }
        }
        public int Year
        {
            get { return year; }
        }
        public string Month
        {
            get { return month; }
        }
        public int ElEnergySpent
        {
            get { return elEnergySpent; }
        }
    }
}
