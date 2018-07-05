﻿using RestaurantReviews.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Xml.Serialization;

namespace RestaurantReviews.ConsoleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            var restaurants = new List<Restaurant>();
            var serializer = new XmlSerializer(typeof(List<Restaurant>));

            Console.WriteLine("Restaurant Reviews");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("r:\tDisplay or modify restaurants.");
                Console.WriteLine("a:\tAdd new restaurant.");
                Console.WriteLine("s:\tSave data to disk.");
                Console.WriteLine("l:\tLoad data from disk.");
                Console.WriteLine();
                Console.Write("Enter valid menu option, or \"q\" to quit: ");
                var input = Console.ReadLine();
                if (input == "r")
                {
                    Console.WriteLine();
                    if (restaurants.Count == 0)
                    {
                        Console.WriteLine("No restaurants.");
                    }
                    while (restaurants.Count > 0)
                    {
                        for (var i = 1; i <= restaurants.Count; i++)
                        {
                            var restaurant = restaurants[i - 1];
                            var restaurantString = $"{i}: \"{restaurant.Name}\"";
                            if (restaurant.Reviews?.Count > 0)
                            {
                                restaurantString += $", with score {restaurant.Score}"
                                    + $" from {restaurant.Reviews.Count} review";
                                if (restaurant.Reviews.Count > 1)
                                {
                                    restaurantString += "s";
                                }
                            }
                            else
                            {
                                restaurantString += ", with no reviews";
                            }
                            Console.WriteLine(restaurantString);
                        }
                        Console.WriteLine();
                        Console.Write("Enter valid menu option, or \"b\" to go back: ");
                        input = Console.ReadLine();
                        if (int.TryParse(input, out var restaurantNum)
                                && restaurantNum > 0 && restaurantNum <= restaurants.Count)
                        {
                            var restaurant = restaurants[restaurantNum - 1];
                            var reviews = restaurant.Reviews;
                            while (true)
                            {
                                Console.WriteLine();
                                var restaurantString = $"\"{restaurant.Name}\"";
                                if (restaurant.Reviews?.Count > 0)
                                {
                                    restaurantString += $", with score {restaurant.Score}"
                                        + $" from {restaurant.Reviews.Count} review";
                                    if (restaurant.Reviews.Count > 1)
                                    {
                                        restaurantString += "s";
                                    }
                                }
                                else
                                {
                                    restaurantString += ", with no reviews";
                                }
                                Console.WriteLine(restaurantString);
                                Console.WriteLine();
                                if (reviews.Count > 0)
                                {
                                    Console.WriteLine("r:\tDisplay reviews.");
                                }
                                Console.WriteLine("a:\tAdd review.");
                                Console.WriteLine("e:\tEdit.");
                                Console.WriteLine("d:\tDelete.");
                                Console.WriteLine();
                                Console.Write("Enter valid menu option, or \"b\" to go back: ");
                                input = Console.ReadLine();
                                if (input == "r" && reviews.Count > 0)
                                {
                                    while (reviews.Count > 0)
                                    {
                                        Console.WriteLine();
                                        for (var i = 1; i <= reviews.Count; i++)
                                        {
                                            var review = reviews[i - 1];
                                            Console.WriteLine($"{i}:"
                                                + $" From \"{review.ReviewerName}\""
                                                + $" with score {review.Score}"
                                                + $" and text \"{review.Text}\"");
                                        }
                                        Console.WriteLine();
                                        Console.Write("Enter valid menu option,"
                                            + " or \"b\" to go back: ");
                                        input = Console.ReadLine();
                                        if (int.TryParse(input, out var reviewNum)
                                            && reviewNum > 0 && reviewNum <= reviews.Count)
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine("e:\tEdit.");
                                            Console.WriteLine("d:\tDelete.");
                                            Console.WriteLine();
                                            Console.Write("Enter valid menu option, "
                                                + "or \"b\" to go back: ");
                                            input = Console.ReadLine();
                                            if (input == "e")
                                            {
                                                var review = reviews[reviewNum - 1];
                                                var newReview = new Review();
                                                while (newReview.ReviewerName == null)
                                                {
                                                    Console.WriteLine();
                                                    Console.WriteLine("Current reviewer's name: "
                                                        + review.ReviewerName);
                                                    Console.WriteLine();
                                                    Console.Write("Enter new reviewer's name: ");
                                                    input = Console.ReadLine();
                                                    try
                                                    {
                                                        newReview.ReviewerName = input;
                                                    }
                                                    catch (ArgumentException ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                                while (newReview.Score == null)
                                                {
                                                    Console.WriteLine();
                                                    Console.WriteLine($"Current score: "
                                                        + review.Score);
                                                    Console.WriteLine();
                                                    Console.Write("Enter new score: ");
                                                    input = Console.ReadLine();
                                                    try
                                                    {
                                                        newReview.Score = int.Parse(input);
                                                    }
                                                    catch (FormatException ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    catch (OverflowException ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                    catch (ArgumentException ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                                while (newReview.Text == null)
                                                {
                                                    Console.WriteLine();
                                                    Console.WriteLine("Current text: " +
                                                        review.Text);
                                                    Console.WriteLine();
                                                    Console.Write("Enter new text: ");
                                                    input = Console.ReadLine();
                                                    try
                                                    {
                                                        newReview.Text = input;
                                                    }
                                                    catch (ArgumentException ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                                reviews[reviewNum - 1] = newReview;
                                            }
                                            else if (input == "d")
                                            {
                                                reviews.RemoveAt(reviewNum - 1);
                                            }
                                            else if (input == "b")
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine();
                                                Console.WriteLine($"Invalid input \"{input}\".");
                                            }
                                        }
                                        else if (input == "b")
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine($"Invalid input \"{input}\".");
                                        }
                                    }
                                }
                                else if (input == "a")
                                {
                                    var newReview = new Review();
                                    while (newReview.ReviewerName == null)
                                    {
                                        Console.WriteLine();
                                        Console.Write("Enter reviewer's name: ");
                                        input = Console.ReadLine();
                                        try
                                        {
                                            newReview.ReviewerName = input;
                                        }
                                        catch (ArgumentException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    while (newReview.Score == null)
                                    {
                                        Console.WriteLine();
                                        Console.Write("Enter score: ");
                                        input = Console.ReadLine();
                                        try
                                        {
                                            newReview.Score = int.Parse(input);
                                        }
                                        catch (FormatException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        catch (OverflowException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        catch (ArgumentException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    while (newReview.Text == null)
                                    {
                                        Console.WriteLine();
                                        Console.Write("Enter text: ");
                                        input = Console.ReadLine();
                                        try
                                        {
                                            newReview.Text = input;
                                        }
                                        catch (ArgumentException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    restaurants[restaurantNum - 1].Reviews.Add(newReview);
                                }
                                else if (input == "e")
                                {
                                    var newRestaurant = new Restaurant();
                                    while (newRestaurant.Name == null)
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine($"Current name: {restaurant.Name}");
                                        Console.WriteLine();
                                        Console.Write("Enter the new restaurant's name: ");
                                        input = Console.ReadLine();
                                        try
                                        {
                                            newRestaurant.Name = input;
                                        }
                                        catch (ArgumentException ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    newRestaurant.Reviews = restaurant.Reviews;
                                }
                                else if (input == "d")
                                {
                                    restaurants.RemoveAt(restaurantNum - 1);
                                    break;
                                }
                                else if (input == "b")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($"Invalid input \"{input}\".");
                                }
                            }
                        }
                        else if (input == "b")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Invalid input \"{input}\".");
                        }
                    }
                }
                else if (input == "a")
                {
                    var restaurant = new Restaurant();
                    while (restaurant.Name == null)
                    {
                        Console.WriteLine();
                        Console.Write("Enter the new restaurant's name: ");
                        input = Console.ReadLine();
                        try
                        {
                            restaurant.Name = input;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    restaurants.Add(restaurant);
                }
                else if (input == "s")
                {
                    Console.WriteLine();
                    try
                    {
                        using (var stream = new FileStream("../../../data.xml", FileMode.Create))
                        {
                            serializer.Serialize(stream, restaurants);
                        }
                        Console.WriteLine("Success.");
                    }
                    catch (SecurityException ex)
                    {
                        Console.WriteLine($"Error while saving: {ex.Message}");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Error while saving: {ex.Message}");
                    }
                }
                else if (input == "l")
                {
                    Console.WriteLine();
                    try
                    {
                        using (var stream = new FileStream("../../../data.xml", FileMode.Open))
                        {
                            restaurants = (List<Restaurant>)serializer.Deserialize(stream);
                        }
                        Console.WriteLine("Success.");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("No saved data found.");
                    }
                    catch (SecurityException ex)
                    {
                        Console.WriteLine($"Error while saving: {ex.Message}");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Error while saving: {ex.Message}");
                    }
                }
                else if (input == "q")
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invalid input \"{input}\".");
                }
            }
        }
    }
}
