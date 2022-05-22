using ex2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ex2.Services
{
    public class RateService : IRateService
    {
        private static List<Rate> rates = new List<Rate>();
        public List<Rate> GetAll() {
            return rates;
        }

        public Rate Get(int id) {
            return rates.Find(x => x.Id == id);
        }

        public void Edit(Rate rate) {
            Rate temp = rates.Find(x => x.Id == rate.Id);
            temp.Score = rate.Score;
            temp.Text = rate.Text;
            temp.Date = DateTime.Now.ToString();
        }
        public void Delete(int id) {
            Rate rate = Get(id);
            rates.Remove(rate);
        }

        public void Add(string text, int score)
        {
            int nextId;
            if(rates.Count != 0){
                nextId = rates.Max(x => x.Id) + 1;
            }
            else
            {
                nextId = 1;
            }
            Rate rate = new Rate() { Id = nextId, Text = text, Score = score, Date = DateTime.Now.ToString() };
            rates.Add(rate);
        }
    }
}
