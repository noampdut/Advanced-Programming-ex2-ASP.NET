using ex2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ex2.Services
{
    public class RateService : IRateService
    {
        private static List<Rate> rates = new List<Rate>();
        public RateService()
        {
            Rate rate1 = new Rate() { Id = 1, Date = "23/05/2022 18:12:02", Score = 5, Text = "great chat!", UserName = "noampdut", };
            Rate rate2 = new Rate() { Id = 2, Date = "24/05/2022 10:11:00", Score = 4, Text = "like this chat", UserName = "admin", };
            rates.Add(rate1);
            rates.Add(rate2);
        }
       
        
        public List<Rate> GetAll() {
            return rates;
        }

        public Rate Get(int id) {
            return rates.Find(x => x.Id == id);
        }

        public void Edit(Rate rate) {
            Rate temp = rates.Find(x => x.Id == rate.Id);
            temp.Date = DateTime.Now.ToString();
        }
        public void Delete(int id) {
            Rate rate = Get(id);
            rates.Remove(rate);
        }

        public void Add(string text, int score, string activeUser)
        {
            int nextId;
            if(rates.Count != 0){
                nextId = rates.Max(x => x.Id) + 1;
            }
            else
            {
                nextId = 1;
            }
            Rate rate = new Rate() { Id = nextId, Text = text, Score = score, Date = DateTime.Now.ToString(), UserName = activeUser };
            rates.Add(rate);
        }
    }
}
