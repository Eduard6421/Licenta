using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;








public class tst : MonoBehaviour
{
    class MenuItem
    {
        protected string name { get; set; }
        protected int price { get; set; }

        public MenuItem(string name, int price)
        {
            this.name = name;
            this.price = price;
        }

        public virtual int GetPrice()
        {
            return price;
        }
    }

    class Pizza : MenuItem
    {
        bool isVegan { get; set; }
        List<string> ingredients { get; set; }

        public override int GetPrice()
        {
            return price + price / 10;
        }

        public Pizza(string pizzaName, int price, bool isVegan,
               List<string> ingredients) : base(pizzaName, price)
        {
            this.isVegan = isVegan;
            this.ingredients = ingredients;
        }
    }

    public static void Main(string[] args)
    {
        MenuItem normalItem = new MenuItem("Soda", 12);
        Pizza pizzaItem = new Pizza("Capricciosa", 12, false, new List<string> { "bacon", "mozzarella" });

        List<MenuItem> newList = new List<MenuItem> { normalItem, pizzaItem };

        Console.WriteLine(newList[0].GetPrice());
        Console.WriteLine(newList[1].GetPrice());
    }
}
