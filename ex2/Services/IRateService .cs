using ex2.Models;
using System.Collections.Generic;

namespace ex2.Services
{
    public interface IRateService
    {
        public List<Rate> GetAll();
        public Rate Get(int id);
        public void Edit(Rate rate);
        public void Delete(int id);
        public void Add(string text, int score, string activeUser);
    }
}
